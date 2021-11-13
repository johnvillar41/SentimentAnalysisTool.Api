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
        public async Task<IEnumerable<CommentModel>> FetchCommentsAsync(int pageSize, int pageNumber, string connectionString)
        {
            var sqlQuery = @"SELECT * FROM CommentsTable
                             ORDER BY CommentId
                             OFFSET (@PageNumber-1)*@RowsOfPage ROWS
                             FETCH NEXT @RowsOfPage ROWS ONLY";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var comments = await connection.QueryAsync<CommentModel>(sqlQuery,
                new { 
                        PageNumber = pageNumber,
                        RowsOfPage = pageSize
                    });
            await transaction.CommitAsync();
            return comments;
        }

        public async Task<bool> SaveCommentsAsync(IEnumerable<CommentModel> comments, string connectionString)
        {
            var sqlQuery = @"INSERT INTO CommentsTable(RecordId, 
                                                       CommentScore, 
                                                       CommentDetail, 
                                                       Date) 
                            VALUES(@RecordId, @CommentScore, @CommentDetail, @Date";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, comments, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}
