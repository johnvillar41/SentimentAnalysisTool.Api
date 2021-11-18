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
        public async Task<bool> AddCorpusWordAsync(CorpusWordModel corpusWord, string connectionString)
        {
            var sqlQuery = @"INSERT INTO CorpusWordsTable(CorpusTypeId,CorpusWord) VALUES(
                                @CorpusTypeId,
                                @CorpusWord
                            )";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, corpusWord, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
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
            var rowsAffected = 0;
            foreach (var corpus in corpusWords)
            {
                rowsAffected += await connection.ExecuteAsync(sqlQuery, corpus, transaction);
            }
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

        public async Task<ICollection<CorpusWordModel>> FetchCorpusWordsAsync(int corpusTypeId, string connectionString)
        {
            var sqlQuery = @"SELECT * FROM CorpusWordsTable WHERE CorpusTypeId = @CorpusTypeId";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var corpusWords = await connection.QueryAsync<CorpusWordModel>(sqlQuery, new { CorpusTypeId = corpusTypeId }, transaction);
            return corpusWords.ToList();
        }
    }
}
