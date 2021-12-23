using Newtonsoft.Json;
using System.Collections.Generic;

namespace SentimentAnalysisTool.Api.Models
{
    public class CorpusTypeViewModel
    {
        public int CorpusTypeId { get; set; }
        public string CorpusTypeName { get; set; }
        
        [JsonProperty("CorpusWordModels")]
        public IEnumerable<CorpusWordViewModel> CorpusWordViewModels { get; set; } = new List<CorpusWordViewModel>();
        
        [JsonProperty("SlangRecordModels")]
        public IEnumerable<SlangRecordViewModel> SlangRecordViewModels { get; set; } = new List<SlangRecordViewModel>();

        [JsonProperty("AbbreviationModels")]
        public IEnumerable<AbbreviationsViewModel> AbbreviationViewModels{ get; set; } = new List<AbbreviationsViewModel>();
    }
}
