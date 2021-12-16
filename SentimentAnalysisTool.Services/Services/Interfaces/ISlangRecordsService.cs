using SentimentAnalysisTool.Data.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ISlangRecordsService
    {
        Task<bool> AddSlangRecordAsync(SlangRecordModel slangRecord, string connectionString);
        Task<bool> AddSlangRecordAsync(IEnumerable<SlangRecordModel> slangRecord, string connectionString);
        Task<bool> AddSlangRecordAsync(IEnumerable<SlangRecordModel> slangRecord, DbTransaction transaction, SqlConnection connection);
        Task<bool> DeleteSlangRecordAsync(int slangRecordId, string connectionString);
        Task<SlangRecordModel> FindSlangRecordAsync(string slangRecord, string connectionString);
    }
}
