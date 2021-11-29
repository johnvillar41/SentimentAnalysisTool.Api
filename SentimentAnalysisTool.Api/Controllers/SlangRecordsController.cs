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
    public class SlangRecordsController : ControllerBase
    {
        private readonly ISlangRecordsService _slangRecordsService;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public SlangRecordsController(
            ISlangRecordsService slangRecordsService,
            IConfiguration configuration,
            ICorpusTypeService corpusTypeService)
        {
            _slangRecordsService = slangRecordsService;
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        //POST: api/SlangRecords
        [HttpPost]
        public async Task<IActionResult> AddSlangRecord([FromBody] SlangRecordViewModel slangRecordViewModel)
        {
            if (slangRecordViewModel == null)
                return NotFound();

            var slangRecordModel = new SlangRecordModel
            {
                CorpusType = await _corpusTypeService.FindCorpusTypeAsync(slangRecordViewModel.CorpusTypeId, ConnectionString),
                SlangName = slangRecordViewModel.SlangName
            };
            var result = await _slangRecordsService.AddSlangRecordAsync(slangRecordModel, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //POST: api/SlangRecords/AddSlangRecords
        [HttpPost]
        [Route("AddSlangRecords")]
        public async Task<IActionResult> AddSlangRecords([FromBody] IEnumerable<SlangRecordViewModel> slangRecordViewModels)
        {
            if (slangRecordViewModels == null)
                return NotFound();

            var slangRecordModelsTasks = slangRecordViewModels.Select(async x => new SlangRecordModel()
            {
                CorpusType = await _corpusTypeService.FindCorpusTypeAsync(x.CorpusTypeId, ConnectionString),
                SlangName = x.SlangName
            });
            var slangRecordModels = await Task.WhenAll(slangRecordModelsTasks);
            var result = await _slangRecordsService.AddSlangRecordAsync(slangRecordModels, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //DELETE: api/SlangRecord/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlangRecord([FromHeader] int id)
        {
            var result = await _slangRecordsService.DeleteSlangRecordAsync(id, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
