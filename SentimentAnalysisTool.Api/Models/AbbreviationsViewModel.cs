using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class AbbreviationsViewModel
    {
        public int CorpusTypeId { get; set; }
        public string Abbreviation { get; set; }
        public string AbbreviationWord { get; set; }
    }
}
