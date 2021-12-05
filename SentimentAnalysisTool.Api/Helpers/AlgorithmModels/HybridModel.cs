using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.AlgorithmModels
{
    public class HybridModel
    {
        /// <summary>
        /// This pertains to the value of the SentimentType
        /// </summary>
        [JsonPropertyName("hybridScore")]
        public string HybridScore { get; set; }
        /// <summary>
        /// This pertains to the numerical value of the computed SentimentScore
        /// </summary>
        [JsonPropertyName("hybridValue")]
        public double HybridValue { get; set; }
    }
}
