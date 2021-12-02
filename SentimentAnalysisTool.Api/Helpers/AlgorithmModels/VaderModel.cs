using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers
{
    public class VaderModel
    {
        private SentimentType _sentimentType;

        [JsonPropertyName("compoundScore")]
        public SentimentType Sentiment
        {
            get
            {
                return _sentimentType;
            }
        }
        public void SetSentiment(string value)
        {
            var sentiment = (SentimentType)Enum.Parse(typeof(SentimentType), value);
            _sentimentType = sentiment;
        }

        [JsonPropertyName("compoundValue")]
        public double CompoundValue { get; set; }

        [JsonPropertyName("negativeValue")]
        public double NegativeValue { get; set; }

        [JsonPropertyName("neutralValue")]
        public double NeutralValue { get; set; }

        [JsonPropertyName("positiveValue")]
        public double PositiveValue { get; set; }        
    }
}
