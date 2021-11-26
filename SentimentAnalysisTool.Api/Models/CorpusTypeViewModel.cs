using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SentimentAnalysisTool.Api.Models
{
    public class CorpusTypeViewModel
    {
        public int CorpusTypeId { get; set; }
        public string CorpusTypeName { get; set; }
        
        [JsonPropertyName("CorpusWordModels")]
        public IEnumerable<CorpusWordViewModel> CorpusWordViewModels { get; set; } = new List<CorpusWordViewModel>();
        
        [JsonPropertyName("SlangRecordModels")]
        public IEnumerable<SlangRecordViewModel> SlangRecordViewModels { get; set; } = new List<SlangRecordViewModel>();
    }
}
