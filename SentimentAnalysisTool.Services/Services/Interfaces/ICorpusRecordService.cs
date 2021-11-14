using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ICorpusRecordService
    {
        Task<bool> AddCorpusRecordAsync(CorpusRecordModel corpus, string connectionString);
        Task<bool> AddCorpusRecordAsync(IEnumerable<CorpusRecordModel> corpuses, string connectionString);
    }
}
