using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Api.Helpers.Enums;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<string> UploadCsvAsync(IFormFile csvFormFile, UploadType uploadType)
        {
            var fileExtension = Path.GetExtension(csvFormFile.FileName);
            var guid = Guid.NewGuid();
            if (fileExtension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase) ||
                fileExtension.Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
            {
                var saveFile = string.Empty;
                if(uploadType == UploadType.Comment)
                    saveFile = Path.Combine(_webHostEnvironment.WebRootPath, @"files\", $"{guid}{csvFormFile.FileName}");
                if(uploadType == UploadType.Slang)
                    saveFile = Path.Combine(_webHostEnvironment.WebRootPath, @"slangs\", $"{guid}{csvFormFile.FileName}");

                using var stream = new FileStream(saveFile, FileMode.Create);
                await csvFormFile.CopyToAsync(stream);
                return saveFile;
            }
            return string.Empty;
        }
    }
}
