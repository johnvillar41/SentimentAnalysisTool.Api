using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysisTool.Data.Models
{
    public class WordFrequencyModel
    {
        public int WordFrequencyId { get; set; }
        public RecordModel Record { get; set; }
        public string Word { get; set; }
        public int WordFrequency { get; set; }
    }
}
