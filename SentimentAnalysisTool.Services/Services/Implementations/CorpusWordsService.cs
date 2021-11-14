﻿using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class CorpusWordsService : ICorpusWordsService
    {
        public async Task<bool> AddCorpusWordsAsync(IEnumerable<CorpusWordModel> corpusWords, string connectionString)
        {
            var sqlQuery = @"INSERT INTO CorpusWordsTable(CorpusTypeId,CorpusWord) VALUES(
                                @CorpusTypeId,
                                @CorpusWord
                            )";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            var rowsAffected = await connection.ExecuteAsync(sqlQuery, corpusWords, transaction);
            await transaction.CommitAsync();
            if (rowsAffected > 0)
                return true;

            return false;
        }
    }
}