using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Data.Models;
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
        public FileHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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
    }
}
