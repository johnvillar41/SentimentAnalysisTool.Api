using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using SentimentAnalysisTool.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public FileHelper(
            IWebHostEnvironment webHostEnvironment,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpClient = httpClient;
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

        public async Task<ICollection<CommentViewModel<T>>> PolarizeCsvFileAsync<T>(string filePath, AlgorithmnType algorithmn)
        {
            var polarizedResults = new List<CommentViewModel<T>>();
            if (filePath.Equals(string.Empty))
                throw new Exception("File path not generated!");

            var application = new Application();
            var workbook = application.Workbooks.Open(filePath, ReadOnly: true, Notify: false);
            var worksheet = workbook.ActiveSheet;
            var usedRange = worksheet.UsedRange;

            for (int i = 2; i <= worksheet.Columns.Count; i++)
            {
                var commentScore = worksheet.Cells[i, 1].Value;
                var polarityScore = worksheet.Cells[i, 2].Value;
                var commentDetail = worksheet.Cells[i, 3].Value;
                var commentDate = worksheet.Cells[i, 4].Value;

                if (commentDetail == null)
                    break;

                var algorithmnModel = await ApplyAlgorithmn<T>(commentDetail, algorithmn);
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
            }
            
            workbook.Close(0);
            application.Quit();            
            return polarizedResults;
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
    }
}
