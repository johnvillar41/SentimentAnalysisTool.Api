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
    }
}
