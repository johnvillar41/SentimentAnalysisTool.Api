﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Helpers;
using SentimentAnalysisTool.Api.Helpers.AlgorithmModels;
using SentimentAnalysisTool.Api.Helpers.Interfaces;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApikeyAuth]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ICorpusRecordService _corpusRecordService;
        private readonly IWordFrequencyService _wordFrequencyService;
        private readonly IRecordService _recordService;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly ISlangRecordsService _slangRecordsService;
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IFileHelper _fileHelper;
        private readonly ITextProcessor _textProcessor;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public RecordsController(
            ICommentService commentService,
            ICorpusRecordService corpusRecordService,
            IWordFrequencyService wordFrequencyService,
            IRecordService recordService,
            ICorpusTypeService corpusTypeService,
            IServiceWrapper serviceWrapper,
            IFileHelper fileHelper,
            IConfiguration configuration)
        {
            _commentService = commentService;
            _corpusRecordService = corpusRecordService;
            _wordFrequencyService = wordFrequencyService;
            _recordService = recordService;
            _corpusTypeService = corpusTypeService;
            _serviceWrapper = serviceWrapper;
            _configuration = configuration;
            _fileHelper = fileHelper;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        //POST: api/Records/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadCsv(
            [FromForm] IFormFile file,
            [FromQuery] AlgorithmnType algorithmnType,
            [FromQuery] bool shouldRemoveSlangs,
            [FromQuery] string corpusType)
        {
            var filePath = "";
            try
            {
                filePath = await _fileHelper.UploadCsvAsync(file);
                if (filePath.Equals(string.Empty))
                    return BadRequest("Error Uploading file!");

                switch (algorithmnType)
                {
                    case AlgorithmnType.SentiWordNet:
                        return Ok(await _fileHelper.PolarizeCsvFileAsync<SentiWordNetModel>(filePath, algorithmnType, shouldRemoveSlangs, corpusType));
                    case AlgorithmnType.Vader:
                        var obj = await _fileHelper.PolarizeCsvFileAsync<VaderModel>(filePath, algorithmnType, shouldRemoveSlangs, corpusType);
                        return Ok(obj);
                    case AlgorithmnType.Hybrid:
                        return Ok(await _fileHelper.PolarizeCsvFileAsync<HybridModel>(filePath, algorithmnType, shouldRemoveSlangs, corpusType));
                }
            }
            catch (HttpRequestException)
            {
                var result = await _fileHelper.DeleteCsvAsync(filePath);
                if (!result)
                    return BadRequest("Error Deleting File!");
            }

            return BadRequest();
        }
        //POST: api/Records
        [HttpPost]
        public async Task<IActionResult> AddRecordUsingVader([FromBody] RecordViewModel<VaderModel> recordViewModel)
        {
            if (recordViewModel == null)
                return NotFound();


            var recordModel = new RecordModel()
            {
                RecordName = recordViewModel.RecordName,
                PositivePercent = recordViewModel.PositivePercent,
                NegativePercent = recordViewModel.NegativePercent
            };

            //Initialization of transaction and connection
            using var connection = await _serviceWrapper.OpenConnectionAsync(ConnectionString);
            using var transaction = await _serviceWrapper.BeginTransactionAsync(connection);

            //Beginning of database transaction
            //Insertion for RecordsTable
            var resultPrimaryKey = await _recordService.AddRecordAsync(recordModel, transaction, connection);
            recordModel.RecordId = resultPrimaryKey;
            if (recordModel.RecordId < 1)
                return BadRequest("Error Adding Record!");

            //Insertion for CommentsTable
            var commentModels = recordViewModel.CommentViewModels.Select(x => new CommentModel()
            {
                CommentId = x.CommentId,
                Record = recordModel,
                CommentScore = x.CommentScore,
                CommentDetail = x.CommentDetail,
                Date = x.Date
            });
            var commentsResult = await _commentService.SaveCommentsAsync(commentModels, transaction, connection);
            if (!commentsResult)
                return BadRequest("Error Adding Comments!");

            //Insertion for CorpusRecordsTable
            var corpusRecordModels = recordViewModel.CorpusRecordViewModels.Select(x => new CorpusRecordModel()
            {
                CorpusRecordId = -1,
                Record = recordModel,
                CorpusType = new CorpusTypeModel()
                {
                    CorpusTypeId = x.CorpusTypeId,
                    CorpusTypeName = _corpusTypeService.FindCorpusTypeAsync(x.CorpusTypeId, ConnectionString).Result.CorpusTypeName,
                    CorpusWords = new List<CorpusWordModel>()
                }
            });
            var corpusRecordServiceResult = await _corpusRecordService.AddCorpusRecordAsync(corpusRecordModels, transaction, connection);
            if (!corpusRecordServiceResult)
                return BadRequest("Error Adding Corpus!");

            //Insertion for WordFrequencyTable
            var wordFrequencyModels = recordViewModel.WordFrequencyViewModels.Select(x => new WordFrequencyModel()
            {
                WordFrequencyId = -1,
                Record = recordModel,
                Word = x.Word,
                WordFrequency = x.WordFrequency
            });
            var wordFrequencyServiceResult = await _wordFrequencyService.AddWordFrequenciesAsync(wordFrequencyModels, transaction, connection);
            if (!wordFrequencyServiceResult)
                return BadRequest("Error Adding WordFrequencies!");

            //Commitment to database transaction
            await _serviceWrapper.CommitTransactionAsync(transaction);
            return Ok();
        }
        //DELETE: api/Records/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord([FromRoute] int id)
        {
            var result = await _recordService.DeleteRecordAsync(id, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //GET: api/Records/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> FetchRecord([FromRoute] int id)
        {
            //Initialize SqlConnection and DbTransaction
            using var connection = await _serviceWrapper.OpenConnectionAsync(ConnectionString);
            using var transaction = await _serviceWrapper.BeginTransactionAsync(connection);

            //FetchModels
            var record = await _recordService.FindRecordAsync(id, transaction, connection);
            var comments = await _commentService.FetchCommentsAsync(10, 1, id, transaction, connection);
            var corpuses = await _corpusRecordService.FetchCorpusRecordAsync(id, transaction, connection);
            var wordFrequencies = await _wordFrequencyService.FetchWordFrequenciesAsync(id, transaction, connection);

            //Build ViewModel
            var recordViewModel = new RecordViewModel<VaderModel>()
            {
                RecordId = id,
                RecordName = record.RecordName,
                PositivePercent = record.PositivePercent,
                NegativePercent = record.NegativePercent,
                CommentViewModels = comments.Select(m => new CommentViewModel<VaderModel>(m)),
                CorpusRecordViewModels = corpuses.Select(m => new CorpusRecordViewModel(m)),
                WordFrequencyViewModels = wordFrequencies.Select(m => new WordFrequencyViewModel(m))
            };

            await _serviceWrapper.CommitTransactionAsync(transaction);
            return Ok(recordViewModel);
        }

    }
}
