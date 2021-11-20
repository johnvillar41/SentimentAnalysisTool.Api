using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
            var procedure = "SaveCorpusWord";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(procedure, corpusWord, transaction, commandType: CommandType.StoredProcedure);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddCorpusWordsAsync(IEnumerable<CorpusWordModel> corpusWords, string connectionString)
        {
            var procedure = "SaveCorpusWord";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = 0;
            foreach (var corpus in corpusWords)
            {
                rowsAffected += await connection.ExecuteAsync(procedure, new
                {
                    corpus.CorpusType.CorpusTypeId,
                    corpus.CorpusWord
                }, transaction, commandType: CommandType.StoredProcedure);
            }
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteCorpusWordAsync(int corpusWordId, string connectionString)
        {
            var procedure = "DeleteCorpusWord";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(procedure, corpusWordId, transaction, commandType: CommandType.StoredProcedure);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<ICollection<CorpusWordModel>> FetchCorpusWordsAsync(int corpusTypeId, string connectionString)
        {
            var procedure = "FetchCorpusWord";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var corpusWords = await connection.QueryAsync<CorpusWordModel>(procedure, new { CorpusTypeId = corpusTypeId }, transaction, commandType: CommandType.StoredProcedure);
            return corpusWords.ToList();
        }
    }
}
