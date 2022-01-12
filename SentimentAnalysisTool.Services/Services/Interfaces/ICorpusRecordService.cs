using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ICorpusRecordService
    {
        Task<bool> AddCorpusRecordAsync(CorpusRecordModel corpus, string connectionString);
        Task<bool> AddCorpusRecordAsync(IEnumerable<CorpusRecordModel> corpuses, string connectionString);
        Task<bool> AddCorpusRecordAsync(IEnumerable<CorpusRecordModel> corpuses, DbTransaction transaction, SqlConnection connection);
        Task<IEnumerable<CorpusRecordModel>> FetchCorpusRecordAsync(int id, DbTransaction transaction, SqlConnection connection);
        Task<string> FetchSynonymousWordAsync(string commentSplit,int corpusTypeId, string connectionString);
    }
}
