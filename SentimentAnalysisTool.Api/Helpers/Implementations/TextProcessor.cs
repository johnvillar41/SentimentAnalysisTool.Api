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
        private readonly ICorpusWordsService _corpusWordsService;
        private string ConnectionString { get; }
        public TextProcessor(
            IConfiguration configuration,
            ISlangRecordsService slangRecordsService,
            IAbbreviationsService abbreviationsService,
            ICorpusWordsService corpusWordsService)
        {
            _configuration = configuration;
            _slangRecordsService = slangRecordsService;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
            _abbreviationsService = abbreviationsService;
            _corpusWordsService = corpusWordsService;
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
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < comment.Length; i++)
            {
                if (i <= totalChars)
                {
                    stringBuilder.Append(comment[i]);
                }
                else
                {
                    stringBuilder.Append("");
                }
            }
            var finalComment = stringBuilder.ToString();
            finalComment = Regex.Replace(finalComment, @"http[^\s]+", "");
            finalComment = Regex.Replace(finalComment, @"[^0-9a-zA-Z]+", " ");

            return finalComment.Trim();
        }

        public async Task<string> ConvertSynonymousWordsAsync(string comment, int corpusTypeId)
        {
            var convertedWord = await _corpusWordsService.ConvertSynonymousCommentAsync(corpusTypeId, comment, ConnectionString);
            return convertedWord;
        }

        public bool CheckCommentHasSubjectMatter(string comment, string subjectMatter)
        {
            return comment.Trim().ToLower().Contains(subjectMatter.Trim().ToLower());
        }
    }
}
