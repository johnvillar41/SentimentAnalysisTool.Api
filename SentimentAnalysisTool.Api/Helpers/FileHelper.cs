using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Range = Microsoft.Office.Interop.Excel.Range;

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
        public ICollection<double> PolarizeCsvFile(string filePath)
        {
            if (filePath.Equals(string.Empty))
                throw new Exception("File path not generated!");

            var application = new Application();
            var workbook = application.Workbooks.Open(filePath);
            var worksheet = workbook.ActiveSheet;
            //Get the used Range
            Range usedRange = worksheet.UsedRange;

            //Iterate the rows in the used range
            foreach (Range row in usedRange.Rows)
            {
                var cellValue = (string)(row.Cells).Value;
            }
            return new List<double>();
        }
        public async Task<VaderModel> ApplyVader(string comment)
        {
            var response = await _httpClient.GetAsync($"{_configuration.GetValue<string>("SentimentAlgorithmnBaseUrl")}/{comment}");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStreamAsync();
            var vaderModel = await JsonSerializer.DeserializeAsync<VaderModel>
                          (responseContent,
                 new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return vaderModel;
        }
    }
}
