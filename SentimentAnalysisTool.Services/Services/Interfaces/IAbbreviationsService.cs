using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface IAbbreviationsService
    {
        Task<bool> AddAbbreviationAsync(int corpusTypeId, AbbreviationModel abbreviation, string connectionString);
        Task<bool> AddAbbreviationAsync(int corpusTypeId, IEnumerable<AbbreviationModel> abbreviations, SqlConnection connection, DbTransaction transaction);
        Task<bool> AddAbbreviationAsync(int corpusTypeId, IEnumerable<AbbreviationModel> abbreviations, string connectionString);
    }
}
