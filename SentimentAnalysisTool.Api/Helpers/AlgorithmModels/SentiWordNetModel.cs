using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.AlgorithmModels
{
    public class SentiWordNetModel
    {
        private SentimentType _sentimentType;

        [JsonPropertyName("polarityScore")]
        public double PolarityScore { get; set; }

        [JsonPropertyName("sentimentScore")]
        public SentimentType Sentiment
        {
            get
            {
                return _sentimentType;
            }
            set
            {
                if (value.Equals(nameof(SentimentType.Neutral)))
                    _sentimentType = SentimentType.Neutral;
                if (value.Equals(nameof(SentimentType.Negative)))
                    _sentimentType = SentimentType.Negative;
                if (value.Equals(nameof(SentimentType.Positive)))
                    _sentimentType = SentimentType.Positive;
            }
        }
    }
}
