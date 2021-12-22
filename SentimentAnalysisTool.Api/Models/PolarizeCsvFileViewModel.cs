using SentimentAnalysisTool.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class PolarizeCsvFileViewModel
    {
        public string FilePath { get; set; }
        public AlgorithmnType Algorithmn { get; set; }
        public bool ShouldConvertSlangs { get; set; }
        public string CorpusType { get; set; }
    }
}
