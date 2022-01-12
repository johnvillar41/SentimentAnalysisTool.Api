using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.Interfaces
{
    public interface ITextProcessor
    {
        string RemoveSpecialChars(string comment, int totalChars);
        Task<string> RemoveSlangWordAsync(string comment, int corpusTypeId);
        Task<string> ConvertAbbreviationToBaseWordAsync(string comment, int corpusTypeId);
        Task<string> ConvertSynonymousWordsAsync(string comment, int corpusTypeId);
        bool CheckCommentHasSubjectMatter(string comment, string subjectMatter);
    }
}
