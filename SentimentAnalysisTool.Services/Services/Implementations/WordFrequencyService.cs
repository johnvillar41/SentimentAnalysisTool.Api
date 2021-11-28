using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class WordFrequencyService : IWordFrequencyService
    {
        public async Task<bool> AddWordFrequenciesAsync(IEnumerable<WordFrequencyModel> wordFrequencies, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_WORD_FREQUENCY;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = 0;
            foreach (var item in wordFrequencies)
            {
                rowsAffected += await connection.ExecuteAsync(procedure,
                    new
                    {
                        item.Record.RecordId,
                        item.Word,
                        item.WordFrequency
                    }, commandType: CommandType.StoredProcedure);
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddWordFrequenciesAsync(IEnumerable<WordFrequencyModel> wordFrequencies, DbTransaction transaction, SqlConnection connection)
        {
            var procedure = StoredProcedures.SP_SAVE_WORD_FREQUENCY;
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
            var rowsAffected = 0;
            foreach (var item in wordFrequencies)
            {
                rowsAffected += await connection.ExecuteAsync(procedure,
                    new
                    {
                        item.Record.RecordId,
                        item.Word,
                        item.WordFrequency
                    }, transaction, commandType: CommandType.StoredProcedure);
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddWordFrequenciesAsync(WordFrequencyModel wordFrequency, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_WORD_FREQUENCY;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = await connection.ExecuteAsync(procedure,
                new
                {
                    wordFrequency.Record.RecordId,
                    wordFrequency.Word,
                    wordFrequency.WordFrequency
                }, commandType: CommandType.StoredProcedure);

            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<IEnumerable<WordFrequencyModel>> FetchWordFrequenciesAsync(int recordId, DbTransaction transaction, SqlConnection connection)
        {
            var procedure = StoredProcedures.SP_FETCH_WORD_FREQUENCIES;
            var wordFrequencies = await connection.QueryAsync<WordFrequencyModel>(procedure,
                new
                {
                    RecordId = recordId
                }, transaction, commandType: CommandType.StoredProcedure);
            return wordFrequencies;
        }
    }
}
