using Newtonsoft.Json;
using SentimentAnalysisTool.Api.Helpers;
using System.Text.Json.Serialization;

namespace SentimentAnalysisTool.Api.Models
{
    public abstract class CsvFileViewModel
    {
        [JsonProperty("algorithmn")]
        [JsonPropertyName("algorithmn")]
        public AlgorithmnType Algorithmn { get; set; }

        [JsonProperty("shouldDeleteSlangs")]
        [JsonPropertyName("shouldDeleteSlangs")]
        public bool ShouldDeleteSlangs { get; set; }

        [JsonProperty("shouldConvertAbbreviations")]
        [JsonPropertyName("shouldConvertAbbreviations")]
        public bool ShouldConvertAbbreviations { get; set; }

        [JsonProperty("corpusType")]
        [JsonPropertyName("corpusType")]
        public string CorpusType { get; set; }

        [JsonProperty("maxNumberOfChars")]
        [JsonPropertyName("maxNumberOfChars")]
        public int MaxNumberOfChars { get; set; }

        [JsonProperty("shouldConvertSynonymns")]
        [JsonPropertyName("shouldConvertSynonymns")]
        public bool ShouldConvertSynonymns { get; set; }
    }
}
