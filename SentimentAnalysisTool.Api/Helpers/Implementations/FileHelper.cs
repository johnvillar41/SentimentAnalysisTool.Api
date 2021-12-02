using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
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
        public async Task<string> UploadCsv(IFormFile csvFormFile)
        {
            var fileExtension = Path.GetExtension(csvFormFile.FileName);
            var guid = Guid.NewGuid();
            if (fileExtension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase))
            {
                var saveFile = Path.Combine(_webHostEnvironment.WebRootPath, @"files\", $"{guid}{csvFormFile.FileName}");
                var stream = new FileStream(saveFile, FileMode.Create);
                await csvFormFile.CopyToAsync(stream);
                return saveFile;
            }
            return string.Empty;
        }
        public async Task<ICollection<T>> PolarizeCsvFile<T>(string filePath, AlgorithmnType algorithmn)
        {
            var polarizedResults = new List<T>();
            if (filePath.Equals(string.Empty))
                throw new Exception("File path not generated!");

            var application = new Application();
            var workbook = application.Workbooks.Open(filePath);
            var worksheet = workbook.ActiveSheet;           
            var usedRange = worksheet.UsedRange;
            
            for (int i = 2; i <= worksheet.Columns.Count; i++)
            {
                var cellValue = worksheet.Cells[i, 3].Value;

                if (cellValue == null)
                    break;

                var grade = await ApplyAlgorithmn<T>(cellValue, algorithmn);
                polarizedResults.Add(grade);                
            }

            workbook.Close(0);
            application.Quit();
            return polarizedResults;
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
