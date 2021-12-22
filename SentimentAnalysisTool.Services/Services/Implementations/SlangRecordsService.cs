﻿using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Office.Interop.Excel;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class SlangRecordsService : ISlangRecordsService
    {
        public async Task<bool> AddSlangRecordAsync(SlangRecordModel slangRecord, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_SLANG_RECORD;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = await connection.ExecuteAsync(procedure, new
            {
                slangRecord.CorpusType.CorpusTypeId,
                slangRecord.SlangName
            }, commandType: CommandType.StoredProcedure);
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddSlangRecordAsync(IFormFile slangRecordsCsv, int corpusTypeId, string connectionString)
        {
            var filePath = SaveCsvFile(slangRecordsCsv);
            var slangRecords = TraverseSlangRecordFile(filePath);

            var procedure = StoredProcedures.SP_SAVE_SLANG_RECORD;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = 0;
            foreach (var item in slangRecords)
            {
                rowsAffected += await connection.ExecuteAsync(procedure, new
                {
                    CorpusTypeId = corpusTypeId,
                    SlangName = item.Key,
                    SlangMeaning = item.Value
                }, commandType: CommandType.StoredProcedure);
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddSlangRecordAsync(IEnumerable<SlangRecordModel> slangRecord, DbTransaction transaction, SqlConnection connection)
        {
            var procedure = StoredProcedures.SP_SAVE_SLANG_RECORD;
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
            var rowsAffected = 0;
            foreach (var item in slangRecord)
            {
                rowsAffected += await connection.ExecuteAsync(procedure, new
                {
                    item.CorpusType.CorpusTypeId,
                    item.SlangName
                }, transaction, commandType: CommandType.StoredProcedure);
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteSlangRecordAsync(int slangRecordId, string connectionString)
        {
            var procedure = StoredProcedures.SP_DELETE_SLANG_RECORD;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = await connection.ExecuteAsync(procedure,
                new
                {
                    SlangRecordsId = slangRecordId
                }, commandType: CommandType.StoredProcedure);
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<SlangRecordModel> FindSlangRecordAsync(string slangRecord, int corpusTypeId, string connectionString)
        {
            //TODO: Convert to Stored Proc
            var sqlQuery = "SELECT * FROM SlangRecordsTable WHERE SlangName = @SlangName AND CorpusTypeId = @CorpusTypeId";
            using var connection = new SqlConnection(connectionString);
            var record = await connection.QueryFirstOrDefaultAsync<SlangRecordModel>(sqlQuery,
                new
                {
                    SlangName = slangRecord,
                    CorpusTypeId = corpusTypeId
                }, commandType: CommandType.Text);
            return record;
        }
        private string SaveCsvFile(IFormFile file)
        {
            throw new NotImplementedException();
        }
        private Dictionary<string,string> TraverseSlangRecordFile(string filePath)
        {
            Dictionary<string, string> slangRecordDictionary = new Dictionary<string, string>();

            var application = new Application();
            var workbook = application.Workbooks.Open(filePath, Notify: false, ReadOnly: true);
            Worksheet worksheet = (Worksheet)workbook.ActiveSheet;
            for (int i = 2; i <= worksheet.Columns.Count; i++)
            {
                var slangRecord = worksheet.Cells[i, 1].ToString();
                var slangDefinition = worksheet.Cells[i, 2].ToString();
                slangRecordDictionary.Add(slangRecord, slangDefinition);
            }

            return slangRecordDictionary;
        }
    }
}
