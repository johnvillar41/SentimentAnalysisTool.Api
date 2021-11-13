using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class WordFrequencyService : IWordFrequencyService
    {
        public async Task<bool> AddWordFrequenciesAsync(IEnumerable<WordFrequencyModel> wordFrequencies,string connectionString)
        {
            var sqlQuery = @"INSERT INTO WordFrequencyTable 
                            VALUES(
                                @RecordId,
                                @Word,
                                @WordFrequency
                            )";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, wordFrequencies);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}
