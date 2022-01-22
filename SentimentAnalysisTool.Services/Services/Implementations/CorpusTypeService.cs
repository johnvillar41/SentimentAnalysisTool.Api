using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class CorpusTypeService : ICorpusTypeService
    {
        public async Task<int> AddCorpusTypeAsync(CorpusTypeModel corpusType, SqlConnection connection, DbTransaction transaction)
        {
            var procedure = StoredProcedures.SP_SAVE_CORPUS_TYPE;
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            var primaryKey = await connection.QuerySingleAsync<int>(procedure,
                new
                {
                    corpusType.CorpusTypeName
                },
                transaction, commandType: CommandType.StoredProcedure);
            return primaryKey;
        }

        public async Task<bool> DeleteCorpusTypeAsync(int corpusTypeId, string connectionString)
        {
            var procedure = StoredProcedures.SP_DELETE_CORPUS_TYPE;
            using var connection = new SqlConnection(connectionString);
            var result = await connection.ExecuteAsync(procedure, new
            {
                CorpusTypeId = corpusTypeId
            }, commandType: CommandType.StoredProcedure);
            if (result > 0)
                return true;
            return false;
        }

        public async Task<IEnumerable<CorpusTypeModel>> FetchCorpusTypesAsync(string connectionString)
        {
            var procedure = StoredProcedures.SP_FETCH_CORPUSTYPES;
            using var connection = new SqlConnection(connectionString);
            var result = await connection.QueryAsync<CorpusTypeModel>(procedure, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<CorpusTypeModel> FindCorpusTypeAsync(int corpusTypeId, string connectionString)
        {
            var procedure = StoredProcedures.SP_FETCH_CORPUS_TYPE;
            using var connection = new SqlConnection(connectionString);
            var result = await connection.QueryFirstAsync<CorpusTypeModel>(procedure, new
            {
                CorpusTypeId = corpusTypeId
            }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<CorpusTypeModel> FindCorpusTypeAsync(string corpusType, string connectionString)
        {
            var sqlQuery = @"SELECT * FROM CorpusTypeTable WHERE CorpusTypeName = @CorpusType";
            using var connection = new SqlConnection(connectionString);
            var result = await connection.QueryFirstOrDefaultAsync<CorpusTypeModel>(sqlQuery, new
            {
                CorpusType = corpusType
            }, commandType: CommandType.Text);
            return result;
        }
    }
}
