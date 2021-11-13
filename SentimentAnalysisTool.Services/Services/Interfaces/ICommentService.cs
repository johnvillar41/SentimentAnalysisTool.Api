using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ICommentService
    {
        Task<bool> SaveComments(IEnumerable<CommentModel> comments, string connectionString);
        Task<IEnumerable<CommentModel>> FetchComments(int pageSize, int pageNumber, string connectionString); 
    }
}
