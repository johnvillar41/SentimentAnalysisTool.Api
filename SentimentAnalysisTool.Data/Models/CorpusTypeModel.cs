using System.Collections.Generic;

namespace SentimentAnalysisTool.Data.Models
{
    public class CorpusTypeModel
    {
        public int CorpusTypeId { get; set; }
        public RecordModel Record { get; set; }
        public string CorpusTypeName { get; set; }       
        public IEnumerable<CorpusWordModel> CorpusWords { get; set; }
    }
}
