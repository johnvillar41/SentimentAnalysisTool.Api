using SentimentAnalysisTool.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ISlangRecordsService
    {
        Task<bool> AddSlangRecordAsync(SlangRecordModel slangRecord, string connectionString);
        Task<bool> AddSlangRecordAsync(IEnumerable<SlangRecordModel> slangRecord, string connectionString);
        Task<bool> DeleteSlangRecordAsync(int slangRecordId, string connectionString);
    }
}
