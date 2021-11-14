using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class WordFrequencyViewModel
    {
        public int WordFrequencyId { get; set; }
        public int RecordId { get; set; }
        public string Word { get; set; }
        public int WordFrequency { get; set; }
    }
}
