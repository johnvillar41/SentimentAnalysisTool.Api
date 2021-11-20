using Dapper;
using Microsoft.AspNetCore.Http;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class CommentService : ICommentService
    {
        /// <summary>
        /// Fetches a paginated result of graded(comments with polarity values) comments from the database
        /// </summary>
        /// <param name="pageSize">
        /// Indicates the amount of comments you want to fecth
        /// </param>
        /// <param name="pageNumber">
        /// Indicates the page number for every pagesize fetched
        /// </param>        
        /// <returns>
        /// Collection of graded comments
        /// </returns>
        public async Task<ICollection<CommentModel>> FetchCommentsAsync(int pageSize, int pageNumber, string connectionString)
        {
            var procedure = "PaginateCommentsTable";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var comments = await connection.QueryAsync<CommentModel>(procedure,
                new
                {
                    PageNumber = pageNumber,
                    RowsOfPage = pageSize
                },
                transaction,
                commandType: CommandType.StoredProcedure);
            await transaction.CommitAsync();
            return (ICollection<CommentModel>)comments;
        }
        /// <summary>
        /// This will save the graded reviews comming from the client application to the database
        /// </summary>
        /// <param name="comments">
        /// List of graded(comments with polarity values) comment reviews
        /// </param>        
        /// <returns>
        /// Whether transaction is successfull or not
        /// </returns>
        public async Task<bool> SaveCommentsAsync(IEnumerable<CommentModel> comments, string connectionString)
        {
            var procedure = "SaveComments";
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = 0;
            foreach (var item in comments)
            {
                rowsAffected += await connection.ExecuteAsync(procedure,
                    new
                    {
                        item.Record.RecordId,
                        item.CommentScore,
                        item.CommentDetail,
                        item.Date
                    }, transaction, commandType: CommandType.StoredProcedure);
            }
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> SaveCommentsAsync(IEnumerable<CommentModel> comments, DbTransaction transaction, SqlConnection connection)
        {
            var procedure = "SaveComments";
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
            var rowsAffected = 0;
            foreach (var item in comments)
            {
                rowsAffected += await connection.ExecuteAsync(procedure,
                    new
                    {
                        item.Record.RecordId,
                        item.CommentScore,
                        item.CommentDetail,
                        item.Date
                    }, transaction, commandType: CommandType.StoredProcedure);
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}
