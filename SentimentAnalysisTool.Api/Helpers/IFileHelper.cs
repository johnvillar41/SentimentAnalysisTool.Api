using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers
{
    public interface IFileHelper
    {
        Task<string> UploadCsv(IFormFile file);
        ICollection<CommentModel> PolarizeCsvFile(string filePath);      
        Task<VaderModel> ApplyVader(string comment);
    }
}
