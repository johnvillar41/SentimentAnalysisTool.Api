using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Helpers;
using SentimentAnalysisTool.Api.Helpers.Enums;
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
    public class CorpusWordsController : ControllerBase
    {
        private readonly ICorpusWordsService _corpusWordsService;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IConfiguration _configuration;
        private readonly IFileHelper _fileHelper;
        private string ConnectionString { get; }
        public CorpusWordsController(
            ICorpusWordsService corpusWordsService,
            IConfiguration configuration,
            ICorpusTypeService corpusTypeService,
            IFileHelper fileHelper)
        {
            _corpusWordsService = corpusWordsService;
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
            _fileHelper = fileHelper;
        }
        //GET: api/CorpusWords/1
        [HttpGet("{corpusTypeId}")]
        public async Task<IActionResult> FetchAllCorpusWords(int corpusTypeId)
        {
            var corpusList = await _corpusWordsService.FetchCorpusWordsAsync(corpusTypeId, ConnectionString);
            if (corpusList.Count == 0)
                return NotFound("Empty Corpus List!");

            return Ok(corpusList);
        }   
        //POST api/CorpusWords/{corpusTypeId}
        [HttpPost("{corpusTypeId}")]
        public async Task<IActionResult> AddCorpusWord([FromForm] IFormFile file, [FromRoute] int corpusTypeId)
        {
            var result = await _fileHelper.UploadCsvAsync(file, UploadType.Corpus);
            var corpusWords = await _fileHelper.TraverseCorpusWordsFileAsync(result, corpusTypeId);
            var slangRecordsResult = await _corpusWordsService.AddCorpusWordsAsync(corpusWords, corpusTypeId, ConnectionString);
            if (slangRecordsResult)
                return Ok();

            return BadRequest();
        }
        //POST api/CorpusWords
        [HttpPost]
        public async Task<IActionResult> AddCorpusWord([FromBody] CorpusWordViewModel corpusWord)
        {
            var corpusWordModel = new CorpusWordModel()
            {
                CorpusWordId = -1,
                CorpusWord = corpusWord.CorpusWord,
                CorpusType = await _corpusTypeService.FindCorpusTypeAsync(corpusWord.CorpusTypeId, ConnectionString)
            };
            var result = await _corpusWordsService.AddCorpusWordsAsync(corpusWordModel, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //DELETE: api/CorpusWords/1
        [HttpDelete("{corpusWordId}")]
        public async Task<IActionResult> DeleteCorpusWord([FromRoute] int corpusWordId)
        {
            var result = await _corpusWordsService.DeleteCorpusWordAsync(corpusWordId, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
