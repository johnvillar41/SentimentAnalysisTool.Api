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
        public async Task<bool> UploadCsv(IFormFile csvFormFile)
        {
            var filePath = Path.GetTempFileName();
            if (csvFormFile.Length > 0)
            {
                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await csvFormFile.CopyToAsync(fileStream);
                return true;
            }
            return false;
        }
        public async Task<ICollection<CommentModel>> PolarizeCsvFile(IFormFile csvFormFile)
        {
            throw new NotImplementedException();
        }
    }
}
