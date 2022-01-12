using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SentimentAnalysisTool.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class UploadCsvFileViewModel : CsvFileViewModel
    {
        [JsonProperty("file")]
        public IFormFile File { get; set; }
        public UploadCsvFileViewModel(UploadCsvFileFormViewModel form)
        {
            File = form.File;
            Algorithmn = (AlgorithmnType)Enum.Parse(typeof(AlgorithmnType), form.Algorithmn);
            ShouldDeleteSlangs = Convert.ToBoolean(form.ShouldDeleteSlangs);
            ShouldConvertAbbreviations = Convert.ToBoolean(form.ShouldConvertAbbreviations);
            CorpusType = form.CorpusType;
            MaxNumberOfChars = int.Parse(form.MaxNumberOfChars);
            ShouldConvertSynonymns = Convert.ToBoolean(form.ShouldConvertSynonyms);
            SubjectMatter = form.SubjectMatter;
        }
        public UploadCsvFileViewModel()
        {

        }
    }
}
