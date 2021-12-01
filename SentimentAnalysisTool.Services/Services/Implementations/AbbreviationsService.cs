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
        ///TODO:
        ///1)Create Stored Procedure - DONE
        ///2)Implement AbbreviationService using Dapper and Stored procedure - DONE
        ///3)Apply Dependency Injection on AbbreviationsService - DONE
        ///4)Add AbbreviationsController - DONE
        ///5)Modify CorpusTypeController (Add call for adding of Abbreviations)
        ///6)Modify CorpusTypeModel (Add parameter for AbbreviationsModel)

        public async Task<bool> AddAbbreviationAsync(int corpusTypeId, AbbreviationModel abbreviation, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_ABBREVIATION;
            using var connection = new SqlConnection(connectionString);
            var result = await connection.ExecuteAsync(procedure, new
            {
                abbreviation.CorpusTypeModel.CorpusTypeId,
                abbreviation.Abbreviation,
                abbreviation.AbbreviationWord
            }, commandType: CommandType.StoredProcedure);

            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> AddAbbreviationAsync(int corpusTypeId, IEnumerable<AbbreviationModel> abbreviations, SqlConnection connection, DbTransaction transaction)
        {
            var procedure = StoredProcedures.SP_SAVE_ABBREVIATION;
            var result = 0;
            foreach (var abbreviation in abbreviations)
            {
                result += await connection.ExecuteAsync(procedure, new
                {
                    abbreviation.CorpusTypeModel.CorpusTypeId,
                    abbreviation.Abbreviation,
                    abbreviation.AbbreviationWord
                }, transaction, commandType: CommandType.StoredProcedure);
            }

            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> AddAbbreviationAsync(int corpusTypeId, IEnumerable<AbbreviationModel> abbreviations, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_ABBREVIATION;
            using var connection = new SqlConnection(connectionString);
            var result = 0;
            foreach (var abbreviation in abbreviations)
            {
                result += await connection.ExecuteAsync(procedure, new
                {
                    abbreviation.CorpusTypeModel.CorpusTypeId,
                    abbreviation.Abbreviation,
                    abbreviation.AbbreviationWord
                }, commandType: CommandType.StoredProcedure);
            }

            if (result > 0)
                return true;

            return false;
        }
    }
}
