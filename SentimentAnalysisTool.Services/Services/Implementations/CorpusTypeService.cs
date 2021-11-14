using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class CorpusTypeService : ICorpusTypeService
    {
        private readonly ICorpusWordsService _corpusWordsService;
        public CorpusTypeService(ICorpusWordsService corpusWordsService)
        {
            _corpusWordsService = corpusWordsService;
        }
        public async Task<bool> AddCorpusTypeAsync(CorpusTypeModel corpusType, string connectionString)
        {
            var sqlQuery = @"INSERT INTO CorpusTypeTable(CorpusRecordsId,CorpusTypeName)
                            VALUES(
                                @CorpusRecordsId,
                                @CorpusTypeName
                            );
                            SELECT Scope_Identity();";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            var primaryKey = await connection.ExecuteAsync(sqlQuery, corpusType, transaction);

            if(corpusType.CorpusWords.Any())
            {
                corpusType.CorpusWords.Select(x => x.CorpusType.CorpusTypeId = primaryKey);
                var corpusWordResult = await _corpusWordsService.AddCorpusWordsAsync(corpusType.CorpusWords, connectionString);
                if (!corpusWordResult)
                    return false;
            }           

            await transaction.CommitAsync();
            if (primaryKey > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteCorpusTypeAsync(int corpusTypeId, string connectionString)
        {
            var sqlQuery = @"DELETE FROM CorpusTypeTable WHERE CorpusTypeId = @CorpusTypeId";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var result = await connection.ExecuteAsync(sqlQuery, new { CorpusTypeId = corpusTypeId }, transaction);
            if (result > 0)
                return true;

            return false;
        }

        public async Task<CorpusTypeModel> FindCorpusTypeAsync(int corpusTypeId, string connectionString)
        {
            var sqlQuery = @"SELECT * FROM CorpusTypeTable WHERE CorpusTypeId = @CorpusTypeId";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var result = await connection.QueryFirstAsync<CorpusTypeModel>(sqlQuery, new { CorpusTypeId = corpusTypeId }, transaction);
            await transaction.CommitAsync();
            return result;
        }
    }
}
