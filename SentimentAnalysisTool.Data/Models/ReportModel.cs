using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysisTool.Data.Models
{
    public class ReportModel
    {
        public int ReportId { get; set; }
        public int PositivePercent { get; set; }
        public int NegativePercent { get; set; }

        public int RecordId { get; set; }
        public RecordModel Record { get; set; }
        public IEnumerable<WordFrequencyModel> WordFrequencies { get; set; }
    }
}
