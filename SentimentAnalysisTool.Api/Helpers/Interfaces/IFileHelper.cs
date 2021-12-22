using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Api.Helpers.AlgorithmModels;
using SentimentAnalysisTool.Api.Helpers.Enums;
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
        Task<string> UploadCsvAsync(IFormFile file, UploadType uploadType);
        Task<bool> DeleteCsvAsync(string filePath);
        Task<IEnumerable<SlangRecordModel>> TraverseSlangRecordFileAsync(string filePath, int corpusTypeId);
        Task<IEnumerable<SlangRecordModel>> TraverseAbbreviationsFileAsync(string filePath, int corpusTypeId);
    }
}
