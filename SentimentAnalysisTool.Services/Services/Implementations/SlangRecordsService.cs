using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class SlangRecordsService : ISlangRecordsService
    {
        public Task<bool> AddSlangRecordAsync(SlangRecordModel slangRecord, string connectionString)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSlangRecordAsync(int slangRecordId, string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
