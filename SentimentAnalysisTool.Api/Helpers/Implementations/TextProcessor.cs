using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Helpers.Interfaces;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.Implementations
{
    public class TextProcessor : ITextProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly ISlangRecordsService _slangRecordsService;
        private readonly IAbbreviationsService _abbreviationsService;
        private string ConnectionString { get; }
        public TextProcessor(
            IConfiguration configuration,
            ISlangRecordsService slangRecordsService,
            IAbbreviationsService abbreviationsService)
        {
            _configuration = configuration;
            _slangRecordsService = slangRecordsService;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
            _abbreviationsService = abbreviationsService;
        }
        public async Task<string> RemoveSlangWordAsync(string comment, int corpusTypeId)
        {
            var commentList = comment.Split(" ").ToList();
            for (int i = 0; i < commentList.Count; i++)
            {
                var slangRecord = await _slangRecordsService.FindSlangRecordAsync(commentList[i], corpusTypeId, ConnectionString);
                if (slangRecord != null)
                {
                    if (commentList[i].Equals(slangRecord.SlangName, StringComparison.OrdinalIgnoreCase))
                    {
                        commentList[i] = string.Empty;
                    }
                }
            }
            var finalComment = string.Join(" ", commentList).Trim();
            return finalComment;
        }

        public async Task<string> ConvertAbbreviationToBaseWordAsync(string comment, int corpusTypeId)
        {
            var commentList = comment.Split(" ").ToList();
            for (int i = 0; i < commentList.Count; i++)
            {
                var abbreviationRecord = await _abbreviationsService.FindAbbreviationAsync(commentList[i], corpusTypeId, ConnectionString);
                if (abbreviationRecord != null)
                {
                    if (commentList[i].Equals(abbreviationRecord.Abbreviation, StringComparison.OrdinalIgnoreCase))
                    {
                        commentList[i] = abbreviationRecord.AbbreviationWord;
                    }
                }
            }
            var finalComment = string.Join(" ", commentList).Trim();
            return finalComment;
        }

        public string RemoveSpecialChars(string comment, int totalChars)
        {
            comment = Regex.Replace(comment, @"http[^\s]+", "");
            comment = Regex.Replace(comment, @"[^0-9a-zA-Z]+", " ");            
                     
            return comment.Trim();
        }
    }
}
