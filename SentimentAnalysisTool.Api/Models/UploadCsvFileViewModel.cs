using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class UploadCsvFileViewModel
    {
        [JsonPropertyName("file")]
        public IFormFile CsvFormFile { get; set; }

        [JsonPropertyName("algorithmn")]
        public AlgorithmnType Algorithmn { get; set; }

        [JsonPropertyName("shouldConvertSlangs")]
        public bool ShouldConvertSlangs { get; set; }

        [JsonPropertyName("corpusType")]
        public string CorpusType { get; set; }
    }
}
