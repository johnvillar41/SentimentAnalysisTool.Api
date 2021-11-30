using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
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
        ///1)Create Stored Procedure
        ///2)Implement AbbreviationService using Dapper and Stored procedure
        ///3)Apply Dependency Injection on AbbreviationsService
        ///4)Add AbbreviationsController
        ///5)Modify CorpusTypeController (Add call for adding of Abbreviations)
        ///6)Modify CorpusTypeModel (Add parameter for AbbreviationsModel)
        
        public Task<bool> AddAbbreviationAsync(int recordId, AbbreviationModel abbreviation, string connectionString)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAbbreviationAsync(int recordId, IEnumerable<AbbreviationModel> abbreviations, SqlConnection connection, DbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAbbreviationAsync(int recordId, IEnumerable<AbbreviationModel> abbreviations, string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
