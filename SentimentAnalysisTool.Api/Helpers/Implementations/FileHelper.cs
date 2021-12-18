using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using SentimentAnalysisTool.Api.Helpers.Interfaces;
using SentimentAnalysisTool.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly ITextProcessor _textProcessor;
        private readonly IConfiguration _configuration;
        public FileHelper(
            IWebHostEnvironment webHostEnvironment,
            HttpClient httpClient,
            ITextProcessor textProcessor,
            IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpClient = httpClient;
            _textProcessor = textProcessor;
            _configuration = configuration;
        }

        public async Task<bool> DeleteCsvAsync(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            if (fileInfo != null && fileInfo.Exists)
            {
                var task = Task.Run(() =>
                {
                    fileInfo.Delete();
                });
                await task;
                return true;
            }
            return false;
        }

        public async Task<RecordViewModel<T>> PolarizeCsvFileAsync<T>(
            string filePath, 
            AlgorithmnType algorithmn, 
            bool shouldRemoveSlangs)
        {
            var polarizedResults = new List<CommentViewModel<T>>();
            var wordFrequencies = new List<WordFrequencyViewModel>();

            if (filePath.Equals(string.Empty))
                throw new Exception("File path not generated!");

            var application = new Application();
            var workbook = application.Workbooks.Open(filePath, Notify: false);
            var worksheet = workbook.ActiveSheet;
            var usedRange = worksheet.UsedRange;

            var positiveInstance = 0;
            var negativeInstance = 0;
            StringBuilder stringBuilder = new();

            //Traverse Excel file
            for (int i = 2; i <= worksheet.Columns.Count; i++)
            {
                var commentScore = worksheet.Cells[i, 1].Value;
                var polarityScore = worksheet.Cells[i, 2].Value;
                var commentDetail = worksheet.Cells[i, 3].Value;
                var commentDate = worksheet.Cells[i, 4].Value;

                if (commentDetail == null)
                    break;

                if (shouldRemoveSlangs)
                {
                    var updatedComment = await _textProcessor.ConvertSlangWordToBaseWordAsync(commentDetail);
                    worksheet.Cells[i, 3] = updatedComment;                    
                }

                var algorithmnModel = await ApplyAlgorithmn<T>(commentDetail, algorithmn);
                CreatePolarizedResults<T>(polarizedResults, ref positiveInstance, ref negativeInstance, stringBuilder, commentScore, polarityScore, commentDetail, commentDate, algorithmnModel);
            }

            await BuildFullStringAsync(wordFrequencies, stringBuilder);

            RecordViewModel<T> recordViewModel = BuildRecordViewModel(polarizedResults, wordFrequencies, application, workbook, positiveInstance, negativeInstance);
            return recordViewModel;
        }

        private void CreatePolarizedResults<T>(List<CommentViewModel<T>> polarizedResults, ref int positiveInstance, ref int negativeInstance, StringBuilder stringBuilder, dynamic commentScore, dynamic polarityScore, dynamic commentDetail, dynamic commentDate, dynamic algorithmnModel)
        {
            polarizedResults.Add(new CommentViewModel<T>
            {
                CommentId = -1,
                RecordId = -1,
                CommentScore = int.Parse(Convert.ToString(commentScore)),
                CommentDetail = Convert.ToString(commentDetail),
                CommentPolarity = Convert.ToString(polarityScore),
                Date = DateTime.Parse(Convert.ToString(commentDate)),
                AlgorithmnModel = algorithmnModel
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

        private async Task BuildFullStringAsync(List<WordFrequencyViewModel> wordFrequencies, StringBuilder stringBuilder)
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

        public async Task<string> UploadCsvAsync(IFormFile csvFormFile)
        {
            var fileExtension = Path.GetExtension(csvFormFile.FileName);
            var guid = Guid.NewGuid();
            if (fileExtension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase) ||
                fileExtension.Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
            {
                var saveFile = Path.Combine(_webHostEnvironment.WebRootPath, @"files\", $"{guid}{csvFormFile.FileName}");
                using var stream = new FileStream(saveFile, FileMode.Create);
                await csvFormFile.CopyToAsync(stream);
                return saveFile;
            }
            return string.Empty;
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
                var positiveSentimentsFile = File.ReadLines(@"C:\Users\Villar\Desktop\SentimentAnalysisToolBackend\SentimentAnalysisTool.Api\wwwroot\sentiment-files\positive-words.txt").ToList();
                var negativeSentimentsFile = File.ReadLines(@"C:\Users\Villar\Desktop\SentimentAnalysisToolBackend\SentimentAnalysisTool.Api\wwwroot\sentiment-files\negative-words.txt");

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
