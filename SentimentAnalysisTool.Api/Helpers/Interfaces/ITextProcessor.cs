using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.Interfaces
{
    public interface ITextProcessor
    {
        Task<string> RemoveSpecialCharsAsync(string comment, int totalChars);
        Task<string> ConvertSlangWordToBaseWordAsync(string comment, int corpusTypeId);
        Task<string> ReplaceAbbreviationAsync(string comment);
    }
}
