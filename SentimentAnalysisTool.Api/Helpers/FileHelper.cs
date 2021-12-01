using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
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
        public async Task<bool> UploadCsv(IFormFile csvFormFile)
        {
            var filePath = await BuildCsvLink(csvFormFile);
            if (filePath == null)
                return false;

            return true;
        }
        public async Task<ICollection<CommentModel>> PolarizeCsvFile(IFormFile csvFormFile)
        {
            var filePath = await BuildCsvLink(csvFormFile);

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
                //Do something with the row.

                //Ex. Iterate through the row's data and put in a string array
                String[] rowData = new String[row.Columns.Count];
                for (int i = 0; i < row.Columns.Count; i++)
                    rowData[i] = Convert.ToString(row.Cells[1, i + 1].Value2);
                //Polarize each comment reviews here
            }
            throw new NotImplementedException();
        }
        private async Task<string> BuildCsvLink(IFormFile csvFormFile)
        {
            var fileExtension = Path.GetExtension(csvFormFile.FileName);
            var guid = Guid.NewGuid();
            if (fileExtension.Equals(".xslx", StringComparison.CurrentCultureIgnoreCase))
            {
                var saveFile = Path.Combine(_webHostEnvironment.WebRootPath, @"files\", $"{guid}{csvFormFile.FileName}");
                var stream = new FileStream(saveFile, FileMode.Create);
                await csvFormFile.CopyToAsync(stream);
                return @$"images\customers\{guid}{csvFormFile.FileName}";
            }
            return null;
        }
        private async Task<VaderModel> ApplyVader(string comment)
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
