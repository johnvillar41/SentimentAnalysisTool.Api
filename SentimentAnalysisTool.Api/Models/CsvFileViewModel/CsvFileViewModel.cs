using Newtonsoft.Json;
using SentimentAnalysisTool.Api.Helpers;

namespace SentimentAnalysisTool.Api.Models
{
    public abstract class CsvFileViewModel
    {
        [JsonProperty("algorithmn")]
        public AlgorithmnType Algorithmn { get; set; }

        [JsonProperty("shouldDeleteSlangs")]
        public bool ShouldDeleteSlangs { get; set; }

        [JsonProperty("shouldConvertAbbreviations")]
        public bool ShouldConvertAbbreviations { get; set; }

        [JsonProperty("corpusType")]
        public string CorpusType { get; set; }

        [JsonProperty("maxNumberOfChars")]
        public int MaxNumberOfChars { get; set; }
    }
}
