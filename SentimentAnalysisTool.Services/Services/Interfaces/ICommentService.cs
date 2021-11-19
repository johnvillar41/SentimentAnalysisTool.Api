using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Data.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface ICommentService
    {
        Task<bool> SaveCommentsAsync(IEnumerable<CommentModel> comments, string connectionString);
        Task<bool> SaveCommentsAsync(IEnumerable<CommentModel> comments, DbTransaction transaction, SqlConnection connection);
        Task<ICollection<CommentModel>> FetchCommentsAsync(int pageSize, int pageNumber, string connectionString);
    }
}
