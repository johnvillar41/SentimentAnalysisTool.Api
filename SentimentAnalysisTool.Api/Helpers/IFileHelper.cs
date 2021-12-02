using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Api.Models;
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
        Task<ICollection<double>> PolarizeCsvFile(string filePath);     
    }
}
