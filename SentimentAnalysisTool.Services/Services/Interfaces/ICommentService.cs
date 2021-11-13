using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ICommentService
    {
        Task<bool> SaveCommentsAsync(IEnumerable<CommentModel> comments, string connectionString);
        Task<IEnumerable<CommentModel>> FetchCommentsAsync(int pageSize, int pageNumber, string connectionString); 
    }
}
