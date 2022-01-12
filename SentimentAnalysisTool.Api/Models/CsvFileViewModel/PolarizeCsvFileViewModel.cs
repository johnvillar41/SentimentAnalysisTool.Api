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
        public PolarizeCsvFileViewModel(UploadCsvFileViewModel file, string filePath)
        {
            FilePath = filePath;
            Algorithmn = file.Algorithmn;
            ShouldDeleteSlangs = file.ShouldDeleteSlangs;
            ShouldConvertAbbreviations = file.ShouldConvertAbbreviations;
            CorpusType = file.CorpusType;
            MaxNumberOfChars = file.MaxNumberOfChars;
            ShouldConvertSynonymns = file.ShouldConvertSynonymns;
            SubjectMatter = file.SubjectMatter;
        }
        public PolarizeCsvFileViewModel()
        {

        }
    }
}
