using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class CorpusRecordViewModel
    {
        public int CorpusRecordsId { get; set; }
        public int RecordId { get; set; }
        public string CorpusName { get; set; }
        public IEnumerable<int> CorpusTypeIds { get; set; }
    }
}
