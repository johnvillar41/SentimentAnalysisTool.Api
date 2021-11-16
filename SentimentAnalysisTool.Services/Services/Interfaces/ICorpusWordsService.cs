using SentimentAnalysisTool.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ICorpusWordsService
    {
        Task<bool> AddCorpusWordsAsync(IEnumerable<CorpusWordModel> corpusWords, string connectionString);
        Task<ICollection<CorpusWordModel>> FetchCorpusWordsAsync(int corpusTypeId, string connectionString);
        Task<bool> DeleteCorpusWordAsync(int corpusWordId, string connectionString);
    }
}
