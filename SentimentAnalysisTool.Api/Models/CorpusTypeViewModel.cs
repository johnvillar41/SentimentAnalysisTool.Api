using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class CorpusTypeViewModel
    {
        public int CorpusRecordId { get; set; }
        public string CorpusTypeName { get; set; }
        public int RecordId { get; set; }
    }
}
