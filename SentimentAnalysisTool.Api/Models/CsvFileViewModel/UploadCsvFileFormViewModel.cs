using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class UploadCsvFileFormViewModel
    {
        public IFormFile File { get; set; }
        public string Algorithmn { get; set; }
        public string ShouldDeleteSlangs { get; set; }
        public string ShouldConvertAbbreviations { get; set; }
        public string ShouldConvertSynonyms { get; set; }
        public string SubjectMatter { get; set; }
        public string CorpusType { get; set; }
        public string MaxNumberOfChars { get; set; }
    }
}
