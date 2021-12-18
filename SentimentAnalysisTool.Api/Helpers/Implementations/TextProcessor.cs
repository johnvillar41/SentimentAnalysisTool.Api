using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Helpers.Interfaces;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.Implementations
{
    public class TextProcessor : ITextProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly ISlangRecordsService _slangRecordsService;
        private string ConnectionString { get; }
        public TextProcessor(IConfiguration configuration, ISlangRecordsService slangRecordsService)
        {
            _configuration = configuration;
            _slangRecordsService = slangRecordsService;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        public async Task<string> ConvertSlangWordToBaseWordAsync(string comment)
        {
            var commentList = comment.Split(" ").ToList();
            for(int i = 0; i < commentList.Count; i++)
            {
                var slangRecord = await _slangRecordsService.FindSlangRecordAsync(commentList[i], ConnectionString);
                if (commentList[i].Equals(slangRecord.SlangName))
                {
                    commentList[i] = slangRecord.SlangMeaning;
                }
            }           
            var finalComment = string.Join(" ", commentList);
            return finalComment;
        }

        public async Task<string> RemoveSpecialCharsAsync(string comment, int totalChars)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReplaceAbbreviationAsync(string comment)
        {
            throw new NotImplementedException();
        }
    }
}
