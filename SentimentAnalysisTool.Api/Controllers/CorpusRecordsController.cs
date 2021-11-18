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
    public class CorpusRecordsController : ControllerBase
    {
        private readonly ICorpusRecordService _corpusRecordService;
        private readonly IRecordService _recordService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public CorpusRecordsController(
            ICorpusRecordService corpusRecordService,
            IRecordService recordService,
            IConfiguration configuration)
        {
            _corpusRecordService = corpusRecordService;
            _recordService = recordService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        //POST: api/CorpusRecords
        [HttpPost]
        public async Task<IActionResult> AddCorpusRecord([FromBody] CorpusRecordViewModel corpusRecordViewModel)
        {
            if (corpusRecordViewModel == null)
                return NotFound("Empty Corpus!");

            var corpusRecordModel = new CorpusRecordModel()
            {
                CorpusRecordId = -1,
                Record = await _recordService.FindRecordAsync(corpusRecordViewModel.RecordId, ConnectionString),               
                CorpusType = null
            };
            var result = await _corpusRecordService.AddCorpusRecordAsync(corpusRecordModel, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
