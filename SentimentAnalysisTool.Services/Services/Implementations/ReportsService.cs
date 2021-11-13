using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class ReportsService : IReportsService
    {
        /// <summary>
        /// This will update the percentages on the Reportstable which will be used to display Piecharts
        /// </summary>
        /// <returns>
        /// Whether update is successfull or not
        /// </returns>
        public async Task<bool> UpdateReportGivenByRecordIdAsync(int recordId, RecordModel record, string connectionString)
        {
            var sqlQuery = @"UPDATE FROM ReportsTable 
                            SET PositivePercent = @PositivePercent,
                                NegativePercent = @NegativePercent
                            WHERE RecordId = @RecordId";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var result = await connection.ExecuteAsync(sqlQuery,
                new
                {
                    PositivePercent = record.PositivePercent,
                    NegativePercent = record.NegativePercent,
                    RecordId = record.RecordId
                });
            await transaction.CommitAsync();
            if (result > 0)
                return true;

            return false;
        }
    }
}
