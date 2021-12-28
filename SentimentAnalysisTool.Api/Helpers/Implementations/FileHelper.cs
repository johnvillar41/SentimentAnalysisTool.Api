using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using SentimentAnalysisTool.Api.Helpers.Enums;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IConfiguration _configuration;

        public FileHelper(
            IWebHostEnvironment webHostEnvironment,
            ICorpusTypeService corpusTypeService,
            IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _corpusTypeService = corpusTypeService;
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
        public async Task<IEnumerable<AbbreviationModel>> TraverseAbbreviationsFileAsync(string filePath, int corpusTypeId)
        {
            List<AbbreviationModel> abbreviations = new();
            var application = new Application();
            var workbook = application.Workbooks.Open(filePath, Notify: false, ReadOnly: true);
            Worksheet worksheet = (Worksheet)workbook.ActiveSheet;
            for (int i = 2; i <= worksheet.Columns.Count; i++)
            {
                if (worksheet.Cells[i, 1].Value == null)
                    break;

                var abbreviation = worksheet.Cells[i, 1].Value;
                var abbreviationMeaning = worksheet.Cells[i, 2].Value;
                abbreviations.Add(new AbbreviationModel()
                {
                    CorpusType = await _corpusTypeService.FindCorpusTypeAsync(corpusTypeId, _configuration.GetConnectionString("SentimentDBConnection")),
                    Abbreviation = abbreviation,
                    AbbreviationWord = abbreviationMeaning
                });
            }

            return abbreviations;
        }
        public async Task<IEnumerable<SlangRecordModel>> TraverseSlangRecordFileAsync(string filePath, int corpusTypeId)
        {
            if (File.Exists(Path.Combine(filePath)))
            {
                var slangNames = File.ReadLines(Path.Combine(filePath)).ToList();
                var slangRecordsTasks = slangNames.Select(async x => new SlangRecordModel()
                {
                    SlangName = x,
                    CorpusType = await _corpusTypeService.FindCorpusTypeAsync(corpusTypeId, _configuration.GetConnectionString("SentimentDBConnection"))
                });
                var slangRecords = await Task.WhenAll(slangRecordsTasks);
                return slangRecords;
            }
            return new List<SlangRecordModel>();
        }

        public async Task<string> UploadCsvAsync(IFormFile csvFormFile, UploadType uploadType)
        {
            var fileExtension = Path.GetExtension(csvFormFile.FileName);
            var guid = Guid.NewGuid();
            if (fileExtension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase) ||
                fileExtension.Equals(".csv", StringComparison.CurrentCultureIgnoreCase) ||
                fileExtension.Equals(".txt", StringComparison.CurrentCultureIgnoreCase))
            {
                var saveFile = string.Empty;
                if (uploadType == UploadType.Comment)
                    saveFile = Path.Combine(_webHostEnvironment.WebRootPath, @"files\", $"{guid}{csvFormFile.FileName}");
                if (uploadType == UploadType.Slang)
                    saveFile = Path.Combine(_webHostEnvironment.WebRootPath, @"slangs\", $"{guid}{csvFormFile.FileName}");
                if (uploadType == UploadType.Abbreviation)
                    saveFile = Path.Combine(_webHostEnvironment.WebRootPath, @"abbreviaitons\", $"{guid}{csvFormFile.FileName}");

                using var stream = new FileStream(saveFile, FileMode.Create);
                await csvFormFile.CopyToAsync(stream);
                return saveFile;
            }
            return string.Empty;
        }
    }
}
