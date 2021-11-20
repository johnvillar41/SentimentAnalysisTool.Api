using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class CorpusTypeService : ICorpusTypeService
    {
        private readonly ICorpusWordsService _corpusWordsService;
        private readonly ISlangRecordsService _slangRecordsService;
        public CorpusTypeService(ICorpusWordsService corpusWordsService, ISlangRecordsService slangRecordsService)
        {
            _corpusWordsService = corpusWordsService;
            _slangRecordsService = slangRecordsService;
        }
        public async Task<bool> AddCorpusTypeAsync(CorpusTypeModel corpusType, string connectionString)
        {
            var procedure = "SaveCorpusType";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            var primaryKey = await connection.QuerySingleAsync<int>(procedure,
                new
                {
                    corpusType.CorpusTypeName
                },
                transaction, commandType: CommandType.StoredProcedure);

            if (corpusType.CorpusWords.Any())
            {
                corpusType.CorpusWords.Select(x => x.CorpusType.CorpusTypeId = primaryKey);
                var corpusWordResult = await _corpusWordsService.AddCorpusWordsAsync(corpusType.CorpusWords, connectionString);
                if (!corpusWordResult)
                    return false;
            }
            if (corpusType.SlangRecords.Any())
            {
                corpusType.SlangRecords.Select(x => x.CorpusType.CorpusTypeId = primaryKey);
                var slangRecordResult = await _slangRecordsService.AddSlangRecordAsync(corpusType.SlangRecords, connectionString);
                if (!slangRecordResult)
                    return false;
            }

            await transaction.CommitAsync();
            if (primaryKey > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteCorpusTypeAsync(int corpusTypeId, string connectionString)
        {
            var procedure = "DeleteCorpusType";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var result = await connection.ExecuteAsync(procedure, new { CorpusTypeId = corpusTypeId }, transaction, commandType: CommandType.StoredProcedure);
            await transaction.CommitAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<CorpusTypeModel> FindCorpusTypeAsync(int corpusTypeId, string connectionString)
        {
            var Procedure = "FetchCorpusType";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var result = await connection.QueryFirstAsync<CorpusTypeModel>(Procedure, new { CorpusTypeId = corpusTypeId }, transaction, commandType: CommandType.StoredProcedure);
            await transaction.CommitAsync();
            return result;
        }
    }
}
