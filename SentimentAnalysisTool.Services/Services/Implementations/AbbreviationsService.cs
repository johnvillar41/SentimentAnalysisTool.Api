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
    public class AbbreviationsService : IAbbreviationsService
    {
        public async Task<bool> AddAbbreviationAsync(AbbreviationModel abbreviation, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_ABBREVIATION;
            using var connection = new SqlConnection(connectionString);
            var result = await connection.ExecuteAsync(procedure, new
            {
                abbreviation.CorpusType.CorpusTypeId,
                abbreviation.Abbreviation,
                abbreviation.AbbreviationWord
            }, commandType: CommandType.StoredProcedure);

            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> AddAbbreviationAsync(IEnumerable<AbbreviationModel> abbreviations, SqlConnection connection, DbTransaction transaction)
        {
            var procedure = StoredProcedures.SP_SAVE_ABBREVIATION;
            var result = 0;
            foreach (var abbreviation in abbreviations)
            {
                result += await connection.ExecuteAsync(procedure, new
                {
                    abbreviation.CorpusType.CorpusTypeId,
                    abbreviation.Abbreviation,
                    abbreviation.AbbreviationWord
                }, transaction, commandType: CommandType.StoredProcedure);
            }

            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> AddAbbreviationAsync(IEnumerable<AbbreviationModel> abbreviations, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_ABBREVIATION;
            using var connection = new SqlConnection(connectionString);
            var result = 0;
            foreach (var abbreviation in abbreviations)
            {
                result += await connection.ExecuteAsync(procedure, new
                {
                    abbreviation.CorpusType.CorpusTypeId,
                    abbreviation.Abbreviation,
                    abbreviation.AbbreviationWord
                }, commandType: CommandType.StoredProcedure);
            }

            if (result > 0)
                return true;

            return false;
        }

        public async Task<IEnumerable<AbbreviationModel>> FetchAbbreviationsAsync(int corpusTypeId, string connectionString)
        {
            var procedure = StoredProcedures.SP_FETCH_ABBREVIATIONS;
            using var connection = new SqlConnection(connectionString);
            var result = await connection.QueryAsync<AbbreviationModel>(procedure, new
            {
                CorpusTypeId = corpusTypeId
            }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<AbbreviationModel> FindAbbreviationAsync(string abbreviation, int corpusTypeId, string connectionString)
        {
            var procedure = StoredProcedures.SP_FETCH_ABBREVIATION;
            using var connection = new SqlConnection(connectionString);
            var result = await connection.QueryFirstOrDefaultAsync<AbbreviationModel>(procedure, new
            {
                CorpusTypeId = corpusTypeId,
                Abbreviation = abbreviation
            }, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
