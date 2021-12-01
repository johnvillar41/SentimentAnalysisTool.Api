using System.Collections.Generic;

namespace SentimentAnalysisTool.Data.Models
{
    public class CorpusTypeModel
    {
        public int CorpusTypeId { get; set; }        
        public string CorpusTypeName { get; set; }       
        public IEnumerable<CorpusWordModel> CorpusWords { get; set; }
        public IEnumerable<SlangRecordModel> SlangRecords { get; set; }
        public IEnumerable<AbbreviationModel> Abbreviations { get; set; }
    }
}
