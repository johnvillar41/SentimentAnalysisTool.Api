using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysisTool.Data.Models
{
    public class CorpusRecordModel
    {
        public int CorpusRecordId { get; set; }
        public RecordModel Record { get; set; }
        public string CorpusName { get; set; }
        public IEnumerable<CorpusTypeModel> CorpusTypes { get; set; }
    }
}
