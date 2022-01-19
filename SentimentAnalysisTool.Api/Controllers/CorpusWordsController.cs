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
    public class CorpusWordsController : ControllerBase
    {
        private readonly ICorpusWordsService _corpusWordsService;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public CorpusWordsController(
            ICorpusWordsService corpusWordsService,
            IConfiguration configuration,
            ICorpusTypeService corpusTypeService)
        {
            _corpusWordsService = corpusWordsService;
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
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
        //POST: api/CorpusWords
        [HttpPost]
        public async Task<IActionResult> AddCorpusWords([FromBody] IEnumerable<CorpusWordViewModel> corpusWords)
        {
            var corpusModelTasks = corpusWords.Select(async x => new CorpusWordModel()
            {
                CorpusWordId = -1,
                CorpusWord = x.CorpusWord,
                CorpusType = await _corpusTypeService.FindCorpusTypeAsync(x.CorpusTypeId, ConnectionString)
            });
            var corpusModels = await Task.WhenAll(corpusModelTasks);
            var result = await _corpusWordsService.AddCorpusWordsAsync(corpusModels, ConnectionString);
            if (result)
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
