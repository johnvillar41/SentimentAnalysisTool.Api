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
        public string SentimentScore { get; set; }
    }
}
