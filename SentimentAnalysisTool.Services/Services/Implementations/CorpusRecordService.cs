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
    public class CorpusRecordService : ICorpusRecordService
    {
        public async Task<bool> AddCorpusRecordAsync(CorpusRecordModel corpus, string connectionString)
        {
            var sqlQuery = @"INSERT INTO CorpusRecordsTable(RecordId,CorpusTypeId)
                            VALUES(
                                @RecordId,
                                @CorpusTypeId
                            )";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, new
            {
                corpus.Record.RecordId,
                corpus.CorpusType.CorpusTypeId
            }, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddCorpusRecordAsync(IEnumerable<CorpusRecordModel> corpuses, string connectionString)
        {
            var sqlQuery = @"INSERT INTO CorpusRecordsTable(RecordId,CorpusTypeId)
                            VALUES(
                                @RecordId,
                                @CorpusTypeId
                            )";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = 0;
            foreach (var corpus in corpuses)
            {
                rowsAffected += await connection.ExecuteAsync(sqlQuery,
                    new
                    {
                        corpus.Record.RecordId,
                        corpus.CorpusType.CorpusTypeId
                    }, transaction);
            }
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddCorpusRecordAsync(IEnumerable<CorpusRecordModel> corpuses, DbTransaction transaction, SqlConnection connection)
        {
            var sqlQuery = @"INSERT INTO CorpusRecordsTable(RecordId,CorpusTypeId)
                            VALUES(
                                @RecordId,
                                @CorpusTypeId
                            )";
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
            var rowsAffected = 0;
            foreach (var corpus in corpuses)
            {
                rowsAffected += await connection.ExecuteAsync(sqlQuery,
                    new
                    {
                        corpus.Record.RecordId,
                        corpus.CorpusType.CorpusTypeId
                    }, transaction);
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}
