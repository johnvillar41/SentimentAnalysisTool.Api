using Dapper;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Implementations
{
    public class CorpusWordsService : ICorpusWordsService
    {
        private readonly ICorpusRecordService _corpusRecordService;
        private readonly ICorpusTypeService _corpusTypeService;
        private HttpClient _httpClient;
        public CorpusWordsService(ICorpusRecordService corpusRecordService, ICorpusTypeService corpusTypeService)
        {
            _httpClient = new HttpClient();
            _corpusRecordService = corpusRecordService;
            _corpusTypeService = corpusTypeService;
        }
        public async Task<bool> AddCorpusWordsAsync(CorpusWordModel corpusWord, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_CORPUS_WORD;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = await connection.ExecuteAsync(procedure, corpusWord, commandType: CommandType.StoredProcedure);
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddCorpusWordsAsync(IEnumerable<CorpusWordModel> corpusWords, string connectionString)
        {
            var procedure = StoredProcedures.SP_SAVE_CORPUS_WORD;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = 0;
            foreach (var corpus in corpusWords)
            {
                rowsAffected += await connection.ExecuteAsync(procedure, new
                {
                    corpus.CorpusType.CorpusTypeId,
                    corpus.CorpusWord
                }, commandType: CommandType.StoredProcedure);
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<bool> AddCorpusWordsAsync(IEnumerable<CorpusWordModel> corpusWords, DbTransaction transaction, SqlConnection connection)
        {
            var procedure = StoredProcedures.SP_SAVE_CORPUS_WORD;
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
            var rowsAffected = 0;
            foreach (var corpus in corpusWords)
            {
                rowsAffected += await connection.ExecuteAsync(procedure, new
                {
                    corpus.CorpusType.CorpusTypeId,
                    corpus.CorpusWord
                }, transaction, commandType: CommandType.StoredProcedure);
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<string> ConvertSynonymousCommentAsync(int corpusId, string word, string connectionString)
        {
            string[] commentSplitted = word.Split(' ');
            StringBuilder stringBuilder = new();
            foreach (var commentSplit in commentSplitted)
            {
                var baseUrl = "http://192.168.1.105:105/";
                var response = await _httpClient.GetAsync($"{baseUrl}/Check/{commentSplit}");

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<bool>
                              (responseContent,
                     new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                if (result)
                {
                    stringBuilder.Append(commentSplit + " ");
                }
                else
                {
                    var wordSynonym = await _corpusRecordService.FetchSynonymousWordAsync(commentSplit, corpusId, connectionString);
                    if (wordSynonym != null)
                        stringBuilder.Append(wordSynonym + " ");
                    else
                        stringBuilder.Append(commentSplit + " ");
                }
            }
            return stringBuilder.ToString().Trim();
        }

        public async Task<bool> DeleteCorpusWordAsync(int corpusWordId, string connectionString)
        {
            var procedure = StoredProcedures.SP_DELETE_CORPUS_WORD;
            using var connection = new SqlConnection(connectionString);
            var rowsAffected = await connection.ExecuteAsync(procedure, corpusWordId, commandType: CommandType.StoredProcedure);
            if (rowsAffected > 0)
                return true;

            return false;
        }

        public async Task<ICollection<CorpusWordModel>> FetchCorpusWordsAsync(int corpusTypeId, string connectionString)
        {
            var procedure = StoredProcedures.SP_FETCH_CORPUS_WORD;
            using var connection = new SqlConnection(connectionString);
            var corpusWords = await connection.QueryAsync<CorpusWordModel>(procedure,
                new
                {
                    CorpusTypeId = corpusTypeId
                }, commandType: CommandType.StoredProcedure);
            corpusWords.ToList().ForEach(async x => x.CorpusType = await _corpusTypeService.FindCorpusTypeAsync(corpusTypeId, connectionString));
            return (ICollection<CorpusWordModel>)corpusWords;
        }
    }
}
