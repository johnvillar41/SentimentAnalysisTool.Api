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
        public async Task<bool> AddCorpusTypeAsync(CorpusTypeModel corpusType, string connectionString)
        {
            var sqlQuery = @"INSERT INTO CorpusTypeTable(CorpusRecordsId,CorpusTypeName)
                            VALUES(
                                @CorpusRecordsId,
                                @CorpusTypeName
                            )";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, corpusType, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}
