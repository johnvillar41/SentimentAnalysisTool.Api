using SentimentAnalysisTool.Data.Models;
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
        public WordFrequencyViewModel(WordFrequencyModel wordFrequencyModel)
        {
            WordFrequencyId = wordFrequencyModel.WordFrequencyId;
            RecordId = wordFrequencyModel.Record.RecordId;
            Word = wordFrequencyModel.Word;
            WordFrequency = wordFrequencyModel.WordFrequency;
        }
    }
}
