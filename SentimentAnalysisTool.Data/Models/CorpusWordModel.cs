using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysisTool.Data.Models
{
    public class CorpusWordModel
    {
        public int CorpusWordId { get; set; }
        public string CorpusWord { get; set; }

        public CorpusTypeModel CorpusType { get; set; }
    }
}
