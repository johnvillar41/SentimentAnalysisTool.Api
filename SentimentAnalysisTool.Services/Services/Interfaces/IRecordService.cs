using SentimentAnalysisTool.Data.Models;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface IRecordService
    {
        Task<int> AddRecordAsync(RecordModel record, string connectionString);
        Task<bool> DeleteRecordAsync(int recordId, string connectionString);
        Task<RecordModel> FindRecordAsync(int recordId, string connectionString);
    }
}
