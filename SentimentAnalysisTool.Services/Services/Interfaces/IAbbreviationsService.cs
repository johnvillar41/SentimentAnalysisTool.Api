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
        Task<bool> AddAbbreviationAsync(AbbreviationModel abbreviation, string connectionString);
        Task<bool> AddAbbreviationAsync(IEnumerable<AbbreviationModel> abbreviations, SqlConnection connection, DbTransaction transaction);
        Task<bool> AddAbbreviationAsync(IEnumerable<AbbreviationModel> abbreviations, string connectionString);
        Task<AbbreviationModel> FindAbbreviationAsync(string abbreviation, int corpusTypeId, string connectionString);
        Task<IEnumerable<AbbreviationModel>> FetchAbbreviationsAsync(int corpusTypeId, string connectionString);
        Task<bool> DeleteAbbreviationAsync(int abbreviationId, string connectionString);
    }
}
