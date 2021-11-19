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
            var sqlQuery = @"INSERT INTO WordFrequencyTable 
                            VALUES(
                                @RecordId,
                                @Word,
                                @WordFrequency
                            )";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = 0;
            foreach (var item in wordFrequencies)
            {
                rowsAffected += await connection.ExecuteAsync(sqlQuery,
                    new
                    {
                        item.Record.RecordId,
                        item.Word,
                        item.WordFrequency
                    }, transaction);
            }

            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddWordFrequenciesAsync(IEnumerable<WordFrequencyModel> wordFrequencies, DbTransaction transaction, SqlConnection connection)
        {
            var sqlQuery = @"INSERT INTO WordFrequencyTable 
                            VALUES(
                                @RecordId,
                                @Word,
                                @WordFrequency
                            )";
            if(connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
            var rowsAffected = 0;
            foreach (var item in wordFrequencies)
            {
                rowsAffected += await connection.ExecuteAsync(sqlQuery,
                    new
                    {
                        item.Record.RecordId,
                        item.Word,
                        item.WordFrequency
                    }, transaction);
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}
