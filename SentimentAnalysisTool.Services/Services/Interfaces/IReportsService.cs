using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface IReportsService
    {
        Task<bool> UpdateReportGivenByRecordIdAsync(int recordId, RecordModel record, string connectionString);
    }
}
