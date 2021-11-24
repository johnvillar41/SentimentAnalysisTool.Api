using SentimentAnalysisTool.Data.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ICorpusWordsService
    {
        Task<bool> AddCorpusWordsAsync(IEnumerable<CorpusWordModel> corpusWords, string connectionString);
        Task<bool> AddCorpusWordsAsync(IEnumerable<CorpusWordModel> corpusWords, DbTransaction transaction, SqlConnection connection);
        Task<bool> AddCorpusWordsAsync(CorpusWordModel corpusWord, string connectionString);
        Task<ICollection<CorpusWordModel>> FetchCorpusWordsAsync(int corpusTypeId, string connectionString);
        Task<bool> DeleteCorpusWordAsync(int corpusWordId, string connectionString);
    }
}
