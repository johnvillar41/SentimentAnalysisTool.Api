using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class RecordService : IRecordService
    {
        public async Task<int> AddRecordAsync(RecordModel record, string connectionString)
        {
            var sqlQuery = @"INSERT INTO RecordsTable(
                                RecordName,
                                PositivePercent,
                                NegativePercent
                            )VALUES(
                                @RecordName,
                                @PositivePercent,
                                @NegativePercent
                            );
                            SELECT SCOPE_IDENTITY();";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var primaryKey = connection.QuerySingle<int>(sqlQuery, record, transaction);
            await transaction.CommitAsync();
            return primaryKey;
        }

        public async Task<bool> DeleteRecordAsync(int recordId, string connectionString)
        {
            var sqlQuery = @"DELETE FROM RecordsTable WHERE RecordId = @RecordId";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = connection.QuerySingle<int>(sqlQuery, recordId, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<RecordModel> FindRecordAsync(int recordId, string connectionString)
        {
            var sqlQuery = @"SELECT * FROM RecordsTable WHERE RecordId = @RecordId";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var record = await connection.QueryFirstOrDefaultAsync<RecordModel>(sqlQuery, new { RecordId = recordId }, transaction);
            return record;
        }
    }
}
