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
    public interface ICorpusTypeService
    {
        Task<int> AddCorpusTypeAsync(CorpusTypeModel corpusType, SqlConnection connection, DbTransaction transaction);
        Task<CorpusTypeModel> FindCorpusTypeAsync(int corpusTypeId, string connectionString);
        Task<bool> DeleteCorpusTypeAsync(int corpusTypeId, string connectionString);
    }
}
