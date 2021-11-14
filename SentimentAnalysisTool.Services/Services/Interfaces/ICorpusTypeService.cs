using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ICorpusTypeService
    {
        Task<bool> AddCorpusTypeAsync(CorpusTypeModel corpusType, string connectionString);
        Task<CorpusTypeModel> FindCorpusTypeAsync(int corpusTypeId, string connectionString);
        Task<bool> DeleteCorpusTypeAsync(int corpusTypeId, string connectionString);
    }
}
