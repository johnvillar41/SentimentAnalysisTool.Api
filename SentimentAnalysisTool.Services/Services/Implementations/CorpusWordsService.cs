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
    public class CorpusWordsService : ICorpusWordsService
    {
        public Task<bool> AddCorpusWordAsync(CorpusWordModel corpusWord, string connectionString)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddCorpusWordsAsync(IEnumerable<CorpusWordModel> corpusWords, string connectionString)
        {
            var sqlQuery = @"INSERT INTO CorpusWordsTable(CorpusTypeId,CorpusWord) VALUES(
                                @CorpusTypeId,
                                @CorpusWord
                            )";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, corpusWords, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteCorpusWordAsync(int corpusWordId, string connectionString)
        {
            var sqlQuery = @"DELETE FROM CorpusWordsTable WHERE CorpusWordId = @CorpusWordId";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, corpusWordId, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public Task<ICollection<CorpusWordModel>> FetchCorpusWordsAsync(int corpusTypeId, string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
