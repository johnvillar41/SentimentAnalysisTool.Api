using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class RecordService : IRecordService
    {
        public async Task<int> AddRecordAsync(RecordModel record, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_RECORDS;
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var primaryKey = connection.QuerySingle<int>(procedure, record, transaction, commandType: CommandType.StoredProcedure);
            await transaction.CommitAsync();
            return primaryKey;
        }

        public async Task<int> AddRecordAsync(RecordModel record, System.Data.Common.DbTransaction transaction, SqlConnection connection)
        {
            var procedure = StoredProcedures.SP_SAVE_RECORDS;
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            var primaryKey = connection.QuerySingle<int>(procedure, record, transaction, commandType: CommandType.StoredProcedure);
            return primaryKey;
        }

        public async Task<bool> DeleteRecordAsync(int recordId, string connectionString)
        {
            var procedure = StoredProcedures.SP_DELETE_RECORDS;
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(procedure, new
            {
                RecordId = recordId
            }, transaction, commandType: CommandType.StoredProcedure);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<RecordModel> FindRecordAsync(int recordId, string connectionString)
        {
            var procedure = StoredProcedures.SP_FETCH_RECORDS;
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var record = await connection.QueryFirstOrDefaultAsync<RecordModel>(procedure, new { RecordId = recordId }, transaction, commandType: CommandType.StoredProcedure);
            return record;
        }
    }
}
