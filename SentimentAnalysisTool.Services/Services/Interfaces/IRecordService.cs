using SentimentAnalysisTool.Data.Models;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface IRecordService
    {
        Task<int> AddRecordAsync(RecordModel record, string connectionString);
        Task<int> AddRecordAsync(RecordModel record, DbTransaction transaction, SqlConnection connection);
        Task<bool> DeleteRecordAsync(int recordId, string connectionString);
        Task<RecordModel> FindRecordAsync(int recordId, string connectionString);
    }
}
