using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class RecordViewModel
    {
        public int RecordId { get; set; }
        public string RecordName { get; set; }
        public int PositivePercent { get; set; }
        public int NegativePercent { get; set; }
        [JsonProperty("CommentModels")]
        public IEnumerable<CommentViewModel> CommentViewModels { get; set; }
        [JsonProperty("CorpusRecordModels")]
        public IEnumerable<CorpusRecordViewModel> CorpusRecordViewModels { get; set; }
        [JsonProperty("WordFrequencyModels")]
        public IEnumerable<WordFrequencyViewModel> WordFrequencyViewModels { get; set; }
    }
}
