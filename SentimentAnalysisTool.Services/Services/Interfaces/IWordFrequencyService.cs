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
    public interface IWordFrequencyService
    {
        Task<bool> AddWordFrequenciesAsync(IEnumerable<WordFrequencyModel> wordFrequencies, string connectionString);
        Task<bool> AddWordFrequenciesAsync(IEnumerable<WordFrequencyModel> wordFrequencies, DbTransaction transaction, SqlConnection connection);
    }
}
