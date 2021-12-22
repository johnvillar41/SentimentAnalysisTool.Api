using SentimentAnalysisTool.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class PolarizeCsvFileViewModel : CsvFileViewModel
    {
        public string FilePath { get; set; }
    }
}
