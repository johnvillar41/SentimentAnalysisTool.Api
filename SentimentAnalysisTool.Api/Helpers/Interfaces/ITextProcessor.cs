using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.Interfaces
{
    public interface ITextProcessor
    {
        string RemoveSpecialCharsAsync(string comment, int totalChars);
        Task<string> ConvertSlangWordToBaseWordAsync(string comment, int corpusTypeId);
        Task<string> ConvertAbbreviationToBaseWordAsync(string comment, int corpusTypeId);
    }
}
