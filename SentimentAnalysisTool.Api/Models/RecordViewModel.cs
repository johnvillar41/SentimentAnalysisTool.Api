using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class RecordViewModel<T>
    {
        public int RecordId { get; set; }
        public string RecordName { get; set; }

        [JsonProperty("positivePercent")]
        [JsonPropertyName("positivePercent")]
        public int PositivePercent { get; set; }

        [JsonProperty("negativePercent")]
        [JsonPropertyName("negativePercent")]
        public int NegativePercent { get; set; }

        [JsonProperty("CommentModels")]
        [JsonPropertyName("CommentModels")]
        public IEnumerable<CommentViewModel<T>> CommentViewModels { get; set; }

        [JsonProperty("CorpusRecordModels")]
        [JsonPropertyName("CorpusRecordModels")]
        public IEnumerable<CorpusRecordViewModel> CorpusRecordViewModels { get; set; }

        [JsonProperty("WordFrequencyModels")]
        [JsonPropertyName("WordFrequencyModels")]
        public IEnumerable<WordFrequencyViewModel> WordFrequencyViewModels { get; set; }
    }
}
