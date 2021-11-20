using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class ServiceWrapper : IServiceWrapper
    {
        public async Task<DbTransaction> BeginTransactionAsync(SqlConnection connection)
        {
            await connection.OpenAsync();
            return await connection.BeginTransactionAsync();
        }

        public async Task CommitTransaction(DbTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public SqlConnection OpenConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
