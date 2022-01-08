using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class WordFrequencyViewModel
    {
        [JsonPropertyName("wordFrequencyId")]
        public int WordFrequencyId { get; set; }

        [JsonPropertyName("recordId")]
        public int RecordId { get; set; }

        [JsonPropertyName("word")]
        public string Word { get; set; }

        [JsonPropertyName("wordFrequency")]
        public int WordFrequency { get; set; }

        [JsonPropertyName("wordType")]
        public string WordType { get; set; }
        public WordFrequencyViewModel(WordFrequencyModel wordFrequencyModel)
        {
            WordFrequencyId = wordFrequencyModel.WordFrequencyId;
            RecordId = wordFrequencyModel.Record.RecordId;
            Word = wordFrequencyModel.Word;
            WordFrequency = wordFrequencyModel.WordFrequency;
        }
        public WordFrequencyViewModel()
        {

        }
    }
}
