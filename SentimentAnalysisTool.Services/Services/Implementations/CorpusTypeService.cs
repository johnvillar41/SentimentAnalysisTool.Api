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

            corpusType.CorpusWords.Select(x => x.CorpusType.CorpusTypeId = primaryKey);
            var corpusWordResult = await _corpusWordsService.AddCorpusWordsAsync(corpusType.CorpusWords, connectionString);
            if (!corpusWordResult)
                return false;

            await transaction.CommitAsync();
            if (primaryKey > 0)
                return true;

            return false;
        }

        public Task<CorpusTypeModel> FindCorpusAsync(int corpusTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
