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
        //POST: api/SlangRecord
        [HttpPost]
        public async Task<IActionResult> AddSlangRecord([FromBody] SlangRecordViewModel slangRecordViewModel)
        {
            if (slangRecordViewModel == null)
                return NotFound();

            var slangRecordModel = BuildSlangRecord(slangRecordViewModel);
            var result = await _slangRecordsService.AddSlangRecordAsync(slangRecordModel, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //POST: api/SlangRecord
        [HttpPost]
        public async Task<IActionResult> AddSlangRecord([FromBody] IEnumerable<SlangRecordViewModel> slangRecordViewModels)
        {
            if (slangRecordViewModels == null)
                return NotFound();

            var slangRecordModels = slangRecordViewModels.Select(async x => new SlangRecordModel()
            {
                CorpusType = await _corpusTypeService.FindCorpusAsync(x.CorpusTypeId),
                SlangName = x.SlangName
            });
            var slangRecords = await Task.WhenAll(slangRecordModels);
            var result = await _slangRecordsService.AddSlangRecordAsync(slangRecords, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //DELETE: api/SlangRecord/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlangRecord(int id)
        {
            var result = await _slangRecordsService.DeleteSlangRecordAsync(id, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }

        private SlangRecordModel BuildSlangRecord(SlangRecordViewModel slangRecordViewModel)
        {
            var slangRecord = new SlangRecordModel
            {
                CorpusType = new CorpusTypeModel { CorpusTypeId = slangRecordViewModel.CorpusTypeId },
                SlangName = slangRecordViewModel.SlangName
            };

            return slangRecord;
        }
    }
}
