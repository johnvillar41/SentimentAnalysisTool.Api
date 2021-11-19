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
    public class SlangRecordsService : ISlangRecordsService
    {
        public async Task<bool> AddSlangRecordAsync(SlangRecordModel slangRecord, string connectionString)
        {
            var sqlQuery = @"INSERT INTO SlangRecordsTable(CorpusTypeId,SlangName)
                            VALUES(
                                @CorpusTypeId,
                                @SlangName
                            )";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, new
            {
                slangRecord.CorpusType.CorpusTypeId,
                slangRecord.SlangName
            }, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddSlangRecordAsync(IEnumerable<SlangRecordModel> slangRecords, string connectionString)
        {
            var sqlQuery = @"INSERT INTO SlangRecordsTable(CorpusTypeId,SlangName)
                            VALUES(
                                @CorpusTypeId,
                                @SlangName
                            )";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = 0;
            foreach (var item in slangRecords)
            {
                rowsAffected += await connection.ExecuteAsync(sqlQuery, new
                {
                    item.CorpusType.CorpusTypeId,
                    item.SlangName
                }, transaction);
            }           
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteSlangRecordAsync(int slangRecordId, string connectionString)
        {
            var sqlQuery = @"DELETE FROM SlangRecordsTable WHERE SlangRecordsId = @SlangRecordsId";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, slangRecordId, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}
