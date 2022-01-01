using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class SlangRecordsService : ISlangRecordsService
    {
        public async Task<bool> AddSlangRecordAsync(SlangRecordModel slangRecord, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_SLANG_RECORD;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = await connection.ExecuteAsync(procedure, new
            {
                slangRecord.CorpusType.CorpusTypeId,
                slangRecord.SlangName
            }, commandType: CommandType.StoredProcedure);
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddSlangRecordAsync(IEnumerable<SlangRecordModel> slangRecords, int corpusTypeId, string connectionString)
        {
            List<int> rowsAffected = new();
            var procedure = StoredProcedures.SP_SAVE_SLANG_RECORD;
            using var connection = new SqlConnection(connectionString);
            foreach (var item in slangRecords)
            {
                rowsAffected.Add(await connection.ExecuteAsync(procedure, new
                {
                    CorpusTypeId = corpusTypeId,
                    item.SlangName
                }, commandType: CommandType.StoredProcedure));
            }
            if (rowsAffected.Contains(1))
                return true;

            return false;
        }

        public async Task<bool> AddSlangRecordAsync(IEnumerable<SlangRecordModel> slangRecord, DbTransaction transaction, SqlConnection connection)
        {
            List<int> rowsAffected = new();
            var procedure = StoredProcedures.SP_SAVE_SLANG_RECORD;
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
            foreach (var item in slangRecord)
            {
                rowsAffected.Add(await connection.ExecuteAsync(procedure, new
                {
                    item.CorpusType.CorpusTypeId,
                    item.SlangName
                }, transaction, commandType: CommandType.StoredProcedure));
            }
            if (rowsAffected.Contains(1))
                return true;

            return false;
        }

        public async Task<bool> DeleteSlangRecordAsync(int slangRecordId, string connectionString)
        {
            var procedure = StoredProcedures.SP_DELETE_SLANG_RECORD;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = await connection.ExecuteAsync(procedure,
                new
                {
                    SlangRecordsId = slangRecordId
                }, commandType: CommandType.StoredProcedure);
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<IEnumerable<SlangRecordModel>> FetchSlangRecordAsync(int? corpusTypeId, string connectionString)
        {
            var procedure = string.Empty;
            if (corpusTypeId == null)
                procedure = StoredProcedures.SP_FETCH_ALL_SLANG_RECORDS;
            else
                procedure = StoredProcedures.SP_FETCH_SLANG_RECORDS;

            using var connection = new SqlConnection(connectionString);
            var records = await connection.QueryAsync<SlangRecordModel>(procedure,
                new
                {
                    CorpusTypeId = corpusTypeId
                }, commandType: CommandType.StoredProcedure);
            return records;
        }

        public async Task<SlangRecordModel> FindSlangRecordAsync(string slangRecord, int corpusTypeId, string connectionString)
        {
            var procedure = StoredProcedures.SP_FETCH_SLANG_RECORD;
            using var connection = new SqlConnection(connectionString);
            var record = await connection.QueryFirstOrDefaultAsync<SlangRecordModel>(procedure,
                new
                {
                    SlangName = slangRecord,
                    CorpusTypeId = corpusTypeId
                }, commandType: CommandType.StoredProcedure);
            return record;
        }
    }
}
