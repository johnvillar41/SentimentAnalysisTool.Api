using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApikeyAuth]
    [ApiController]
    public class CorpusTypeController : ControllerBase
    {
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly ICorpusWordsService _corpusWordsService;
        private readonly ISlangRecordsService _slangRecordsService;
        private readonly IAbbreviationsService _abbreviationsService;
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public CorpusTypeController(
            ICorpusTypeService corpusTypeService,
            IConfiguration configuration,
            IServiceWrapper serviceWrapper,
            ICorpusWordsService corpusWordsService,
            ISlangRecordsService slangRecordsService,
            IAbbreviationsService abbreviationsService)
        {
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            _corpusWordsService = corpusWordsService;
            _serviceWrapper = serviceWrapper;
            _abbreviationsService = abbreviationsService;
            _slangRecordsService = slangRecordsService;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        //POST: api/CorpusType
        [HttpPost]
        public async Task<IActionResult> AddCorpusType([FromBody] CorpusTypeViewModel corpusTypeViewModel)
        {
            if (corpusTypeViewModel == null)
                return NotFound();

            //Build CorpusTypeModel
            var corpusModel = new CorpusTypeModel()
            {
                CorpusTypeId = -1,
                CorpusTypeName = corpusTypeViewModel.CorpusTypeName,
            };
            var corpusWordTasks = corpusTypeViewModel.CorpusWordViewModels.Select(async x => new CorpusWordModel()
            {
                CorpusType = await _corpusTypeService.FindCorpusTypeAsync(x.CorpusTypeId, ConnectionString),
                CorpusWord = x.CorpusWord
            });
            var slangRecordTasks = corpusTypeViewModel.SlangRecordViewModels.Select(async x => new SlangRecordModel()
            {
                CorpusType = await _corpusTypeService.FindCorpusTypeAsync(x.CorpusTypeId, ConnectionString),
                SlangName = x.SlangName
            });
            var abbreviationTasks = corpusTypeViewModel.AbbreviationViewModels.Select(async x => new AbbreviationModel()
            {
                AbbreviationsId = -1,
                CorpusType = await _corpusTypeService.FindCorpusTypeAsync(x.CorpusTypeId, ConnectionString),
                Abbreviation = x.Abbreviation,
                AbbreviationWord = x.AbbreviationWord
            });

            var corpusWords = await Task.WhenAll(corpusWordTasks);
            var slangRecords = await Task.WhenAll(slangRecordTasks);
            var abbreviations = await Task.WhenAll(abbreviationTasks);

            corpusModel.CorpusWords = corpusWords;
            corpusModel.SlangRecords = slangRecords;
            corpusModel.Abbreviations = abbreviations;

            //Initialize Connection and Transaction
            using var connection = await _serviceWrapper.OpenConnectionAsync(ConnectionString);
            using var transaction = await _serviceWrapper.BeginTransactionAsync(connection);

            //Insert CorpusType
            var resultPrimaryKey = await _corpusTypeService.AddCorpusTypeAsync(corpusModel, connection, transaction);
            if (corpusModel.CorpusWords.Any())
            {
                //Insert CorpusWords
                _ = corpusModel.CorpusWords.Select(x => x.CorpusType.CorpusTypeId = resultPrimaryKey);
                var corpusWordResult = await _corpusWordsService.AddCorpusWordsAsync(corpusModel.CorpusWords, transaction, connection);
                if (!corpusWordResult)
                    return BadRequest();
            }
            if (corpusModel.SlangRecords.Any())
            {
                //Insert SlangWords
                _ = corpusModel.SlangRecords.Select(x => x.CorpusType.CorpusTypeId = resultPrimaryKey);
                var slangRecordResult = await _slangRecordsService.AddSlangRecordAsync(corpusModel.SlangRecords, transaction, connection);
                if (!slangRecordResult)
                    return BadRequest();
            }
            if (corpusModel.Abbreviations.Any())
            {
                //Insert Abbreviations
                _ = corpusModel.Abbreviations.Select(x => x.CorpusType.CorpusTypeId = resultPrimaryKey);
                var slangRecordResult = await _abbreviationsService.AddAbbreviationAsync(corpusModel.Abbreviations, connection, transaction);
                if (!slangRecordResult)
                    return BadRequest();
            }

            //Commit Transaction
            await _serviceWrapper.CommitTransactionAsync(transaction);
            return Ok();
        }
        //DELETE: api/CorpusType/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorpusType([FromHeader] int id)
        {
            var result = await _corpusTypeService.DeleteCorpusTypeAsync(id, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //GET: api/CorpusType
        [HttpGet]
        public async Task<IActionResult> FetchCorpusTypes()
        {
            var result = await _corpusTypeService.FetchCorpusTypesAsync(ConnectionString);
            if (result != null)
                return Ok(result);

            return BadRequest();
        }
    }
}
