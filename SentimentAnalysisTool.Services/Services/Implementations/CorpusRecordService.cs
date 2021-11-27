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
            var procedure = StoredProcedures.SP_SAVE_CORPUS_RECORDS;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = await connection.ExecuteAsync(procedure, new
            {
                corpus.Record.RecordId,
                corpus.CorpusType.CorpusTypeId
            }, commandType: CommandType.StoredProcedure);
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddCorpusRecordAsync(IEnumerable<CorpusRecordModel> corpuses, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_CORPUS_RECORDS;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = 0;
            foreach (var corpus in corpuses)
            {
                rowsAffected += await connection.ExecuteAsync(procedure,
                    new
                    {
                        corpus.Record.RecordId,
                        corpus.CorpusType.CorpusTypeId
                    }, commandType: CommandType.StoredProcedure);
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddCorpusRecordAsync(IEnumerable<CorpusRecordModel> corpuses, DbTransaction transaction, SqlConnection connection)
        {
            var sqlQuery = StoredProcedures.SP_SAVE_CORPUS_RECORDS;
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
                    }, transaction, commandType: CommandType.StoredProcedure);
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}
