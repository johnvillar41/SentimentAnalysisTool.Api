using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Helpers.Interfaces;
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
        private string ConnectionString { get; }
        public TextProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        public async Task<string> RemoveSlangWordAsync(string comment)
        {
            return await Task.Run(() =>
            {
                //Sample slangs should be inside database
                var sampleSlangList = new List<string>
                {
                    "hahabells",
                    "huhubells",
                    "ratbu"
                };
                var commentList = comment.Split(" ").ToList();
                foreach (var commentItem in commentList)
                {
                    if (sampleSlangList.Contains(commentItem))
                    {
                        commentList.Remove(commentItem);
                    }
                }
                var finalComment = string.Join(" ", commentList);
                return finalComment;
            });
        }

        public async Task<string> RemoveSpecialCharsAsync(string comment, int totalChars)
        {
            return await Task.Run(() =>
            {
                //Sample chars should be inside database
                var sampleCharsList = new List<string>
                {
                    "hahabells",
                    "huhubells",
                    "ratbu"
                };
                var commentList = comment.Split(" ").ToList();
                foreach (var commentItem in commentList)
                {
                    if (sampleCharsList.Contains(commentItem))
                    {
                        commentList.Remove(commentItem);
                    }
                }
                var finalComment = string.Join(" ", commentList);
                return finalComment;
            });
        }

        public Task<string> ReplaceAbbreviationAsync(string comment)
        {
            throw new NotImplementedException();
        }
    }
}
