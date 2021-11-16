using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class CorpusTypeViewModel
    {
        public int CorpusTypeId { get; set; }
        public string CorpusTypeName { get; set; }
        public IEnumerable<CorpusWordViewModel> CorpusWordViewModels { get; set; } = new List<CorpusWordViewModel>();
        public IEnumerable<SlangRecordViewModel> SlangRecordViewModels { get; set; } = new List<SlangRecordViewModel>();
    }
}
