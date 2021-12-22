using SentimentAnalysisTool.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public abstract class CsvFileViewModel
    {
        [JsonPropertyName("algorithmn")]
        public AlgorithmnType Algorithmn { get; set; }

        [JsonPropertyName("shouldConvertSlangs")]
        public bool ShouldConvertSlangs { get; set; }

        [JsonPropertyName("corpusType")]
        public string CorpusType { get; set; }
    }
}
