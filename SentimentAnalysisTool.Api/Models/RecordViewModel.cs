using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class RecordViewModel<T> where T : class
    {
        public int RecordId { get; set; }
        public string RecordName { get; set; }
        public int PositivePercent { get; set; }
        public int NegativePercent { get; set; }

        [JsonPropertyName("CommentModels")]
        public IEnumerable<CommentViewModel<T>> CommentViewModels { get; set; }

        [JsonPropertyName("CorpusRecordModels")]
        public IEnumerable<CorpusRecordViewModel> CorpusRecordViewModels { get; set; }

        [JsonPropertyName("WordFrequencyModels")]
        public IEnumerable<WordFrequencyViewModel> WordFrequencyViewModels { get; set; }
    }
}
