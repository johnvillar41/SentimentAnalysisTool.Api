using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers
{
    public class VaderModel
    {
        public SentimentType Sentiment { get; set; }
        public double CompoundValue { get; set; }
        public double NegativeValue { get; set; }
        public double NeutralValue { get; set; }
        public double PositiveValue { get; set; }
    }
}
