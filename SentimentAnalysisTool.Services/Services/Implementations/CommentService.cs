using Dapper;
using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class CommentService : ICommentService
    {
        public async Task<bool> SaveComments(IEnumerable<CommentModel> comments, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            IDbTransaction transaction = connection.BeginTransaction();
            var insertedRows = 0;
            foreach(var comment in comments)
            {
                insertedRows = await connection.ExecuteAsync("INSERT INTO CommentsTable(RecordId,CommentScore,CommentDetail,Date) VALUES(@RecordId,@CommentScore,@CommentDetail,@Date)",
                    new { 
                        RecordId = comment.Record.RecordId,
                        CommentScore = comment.CommentScore,
                        CommentDetail = comment.CommentDetail,
                        Date = comment.Date
                    });
            }
            if (insertedRows > 0)
                return true;

            return false;
        }
    }
}
