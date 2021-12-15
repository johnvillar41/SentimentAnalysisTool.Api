using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<string> RemoveSlangWordAsync(string comment)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveSpecialCharsAsync(string comment, int totalChars)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReplaceAbbreviationAsync(string comment)
        {
            throw new NotImplementedException();
        }
    }
}
