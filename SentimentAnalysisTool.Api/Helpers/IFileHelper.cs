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
        Task<bool> UploadCsv(IFormFile file);
        Task<ICollection<CommentModel>> PolarizeCsvFile(IFormFile csvFormFile);
    }
}
