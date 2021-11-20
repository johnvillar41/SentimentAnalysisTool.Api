﻿using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface IServiceWrapper
    {
        SqlConnection OpenConnection(string connectionString);
        Task<DbTransaction> BeginTransactionAsync(SqlConnection connection);
        Task CommitTransaction(DbTransaction transaction);
    }
}
