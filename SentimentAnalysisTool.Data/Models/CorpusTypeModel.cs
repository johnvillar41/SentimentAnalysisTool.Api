using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysisTool.Data.Models
{
    public class CorpusTypeModel
    {
        public int CorpusTypeId { get; set; }
        public string CorpusName { get; set; }
       
        public IEnumerable<CorpusWordModel> CorpusWords { get; set; }
    }
}
