using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorpusWordsController : ControllerBase
    {
        private readonly ICorpusWordsService _corpusWordsService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public CorpusWordsController(ICorpusWordsService corpusWordsService, IConfiguration configuration)
        {
            _corpusWordsService = corpusWordsService;
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
    }
}
