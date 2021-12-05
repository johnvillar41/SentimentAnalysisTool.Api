using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Api.Helpers.AlgorithmModels;
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
        Task<string> UploadCsvAsync(IFormFile file);
        Task<ICollection<CommentViewModel<T>>> PolarizeCsvFileAsync<T>(string filePath, AlgorithmnType algorithmn);
        Task<bool> DeleteCsvAsync(string filePath);
    }
}
