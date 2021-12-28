using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using SentimentAnalysisTool.Api.Helpers.Interfaces;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.Implementations
{
    public class Polarizer : IPolarizer
    {
        private readonly HttpClient _httpClient;
        private readonly ITextProcessor _textProcessor;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Polarizer(
            HttpClient httpClient, 
            ITextProcessor textProcessor, 
            ICorpusTypeService corpusTypeService, 
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _textProcessor = textProcessor;
            _corpusTypeService = corpusTypeService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<RecordViewModel<T>> PolarizeCsvFileAsync<T>(PolarizeCsvFileViewModel polarizeCsvFileViewModel)
        {
            var polarizedResults = new List<CommentViewModel<T>>();
            var wordFrequencies = new List<WordFrequencyViewModel>();

            if (polarizeCsvFileViewModel.FilePath.Equals(string.Empty))
                throw new Exception("File path not generated!");

            var application = new Application();
            var workbook = application.Workbooks.Open(polarizeCsvFileViewModel.FilePath, Notify: false, ReadOnly: true);
            var worksheet = workbook.ActiveSheet;
            var usedRange = worksheet.UsedRange;

            var positiveInstance = 0;
            var negativeInstance = 0;
            StringBuilder stringBuilder = new();

            //Find CorpusTypeId
            var corpusTypeModel = await _corpusTypeService.FindCorpusTypeAsync(polarizeCsvFileViewModel.CorpusType, _configuration.GetConnectionString("SentimentDBConnection"));

            //Traverse Excel file
            for (int i = 2; i <= worksheet.Columns.Count; i++)
            {
                var commentScore = worksheet.Cells[i, 1].Value;
                var polarityScore = worksheet.Cells[i, 2].Value;
                var commentDetail = worksheet.Cells[i, 3].Value;
                var commentDate = worksheet.Cells[i, 4].Value;
                var manualTransformedComment = worksheet.Cells[i, 6].Value;

                if (commentDetail == null)
                    break;

                var updatedComment = string.Empty;

                //Removal of SpecialCharacters
                updatedComment = _textProcessor.RemoveSpecialChars(commentDetail, polarizeCsvFileViewModel.MaxNumberOfChars);

                //Deletion of Slang Words
                if (polarizeCsvFileViewModel.ShouldDeleteSlangs)
                    updatedComment = await _textProcessor.RemoveSlangWordAsync(updatedComment, corpusTypeModel.CorpusTypeId);

                //Convertion of Abbreviation Words
                if (polarizeCsvFileViewModel.ShouldConvertAbbreviations)
                    updatedComment = await _textProcessor.ConvertAbbreviationToBaseWordAsync(updatedComment, corpusTypeModel.CorpusTypeId);

                //Polarization using CorpusWords
                //TODO

                //Polarization of the updated comment
                dynamic algorithmnModel = null;
                if (updatedComment == string.Empty)
                    algorithmnModel = await ApplyAlgorithmn<T>(commentDetail, polarizeCsvFileViewModel.Algorithmn);
                else
                    algorithmnModel = await ApplyAlgorithmn<T>(updatedComment, polarizeCsvFileViewModel.Algorithmn);

                CreatePolarizedResults<T>(polarizedResults, ref updatedComment, ref positiveInstance, ref negativeInstance, stringBuilder, commentScore, polarityScore, commentDetail, commentDate, algorithmnModel, manualTransformedComment);
            }

            await BuildWordFrequencyModels(wordFrequencies, stringBuilder);

            RecordViewModel<T> recordViewModel = BuildRecordViewModel(polarizedResults, wordFrequencies, application, workbook, positiveInstance, negativeInstance);
            return recordViewModel;
        }


        private void CreatePolarizedResults<T>(List<CommentViewModel<T>> polarizedResults, ref string updatedComment, ref int positiveInstance, ref int negativeInstance, StringBuilder stringBuilder, dynamic commentScore, dynamic polarityScore, dynamic commentDetail, dynamic commentDate, dynamic algorithmnModel, dynamic manualTransformedComment)
        {
            if (updatedComment.Equals(string.Empty))
                updatedComment = commentDetail;

            polarizedResults.Add(new CommentViewModel<T>
            {
                CommentId = -1,
                RecordId = -1,
                CommentScore = int.Parse(Convert.ToString(commentScore)),
                CommentDetail = Convert.ToString(commentDetail),
                CommentPolarity = Convert.ToString(polarityScore),
                Date = DateTime.Parse(Convert.ToString(commentDate)),
                AlgorithmnModel = algorithmnModel,
                TransformedComment = updatedComment,
                ManualTransformedComment = Convert.ToString(manualTransformedComment)
            });
            stringBuilder.Append(commentDetail);

            Type type = algorithmnModel.GetType();
            PropertyInfo prop = type.GetProperty("SentimentResult");
            object value = prop.GetValue(algorithmnModel);
            if (value.ToString().Equals("Positive"))
                positiveInstance++;
            else
                negativeInstance++;
        }

        private async Task BuildWordFrequencyModels(List<WordFrequencyViewModel> wordFrequencies, StringBuilder stringBuilder)
        {
            //Build fullstring here
            SortedDictionary<string, int> wordFrequencyDictionary = await CalculateWordFrequency(stringBuilder.ToString());
            foreach (var item in wordFrequencyDictionary)
            {
                wordFrequencies.Add(new WordFrequencyViewModel()
                {
                    RecordId = -1,
                    Word = item.Key,
                    WordFrequency = item.Value
                });
            }
        }

        private RecordViewModel<T> BuildRecordViewModel<T>(List<CommentViewModel<T>> polarizedResults, List<WordFrequencyViewModel> wordFrequencies, Application application, Workbook workbook, int positiveInstance, int negativeInstance)
        {
            //Build RecordViewModel
            double positivePercent = ((double)positiveInstance / (positiveInstance + negativeInstance)) * 100;
            double negativePercent = ((double)negativeInstance / (positiveInstance + negativeInstance)) * 100;
            var recordViewModel = new RecordViewModel<T>()
            {
                RecordName = null,
                PositivePercent = (int)positivePercent,
                NegativePercent = (int)negativePercent,
                CommentViewModels = polarizedResults,
                CorpusRecordViewModels = null,
                WordFrequencyViewModels = wordFrequencies
            };
            workbook.Close(0);
            application.Quit();
            return recordViewModel;
        }



        private async Task<T> ApplyAlgorithmn<T>(string comment, AlgorithmnType algorithmnType)
        {
            var baseUrl = _configuration.GetValue<string>("SentimentAlgorithmnBaseUrl");
            var response = await _httpClient.GetAsync($"{baseUrl}/{algorithmnType}/{comment}");

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStreamAsync();
            var jsonModel = await JsonSerializer.DeserializeAsync<T>
                          (responseContent,
                 new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return jsonModel;
        }
        private async Task<SortedDictionary<string, int>> CalculateWordFrequency(string comments)
        {
            return await Task.Run((Func<SortedDictionary<string, int>>)(() =>
            {
                SortedDictionary<string, int> mp = new();
                string[] commentSplitted = comments.Split(' ');
                var positiveSentimentsFile = File.ReadLines(Path.Combine(_webHostEnvironment.WebRootPath, @"sentiment-files\", "positive-words.txt"));
                var negativeSentimentsFile = File.ReadLines(Path.Combine(_webHostEnvironment.WebRootPath, @"sentiment-files\", "negative-words.txt"));

                for (int i = 0; i < commentSplitted.Length; i++)
                {
                    if (positiveSentimentsFile.Contains(commentSplitted[i]) || negativeSentimentsFile.Contains(commentSplitted[i]))
                    {
                        if (mp.ContainsKey(commentSplitted[i]))
                        {
                            mp[commentSplitted[i]] = mp[commentSplitted[i]] + 1;
                        }
                        else
                        {
                            mp.Add(commentSplitted[i], 1);
                        }
                    }
                }
                return mp;
            }));
        }
    }
}
