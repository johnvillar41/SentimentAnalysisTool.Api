using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
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
    public class SlangRecordsController : ControllerBase
    {
        private readonly ISlangRecordsService _slangRecordsService;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IFileHelper _fileHelper;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public SlangRecordsController(
            ISlangRecordsService slangRecordsService,
            IConfiguration configuration,
            ICorpusTypeService corpusTypeService,
            IFileHelper fileHelper)
        {
            _slangRecordsService = slangRecordsService;
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            _fileHelper = fileHelper;
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
        //POST: api/SlangRecords/AddSlangRecords/{corpusTypeId}
        [HttpPost("AddSlangRecords/{corpusTypeId}")]
        public async Task<IActionResult> AddSlangRecords([FromForm] IFormFile file, [FromRoute] int corpusTypeId)
        {
            var result = await _fileHelper.UploadCsvAsync(file, UploadType.Slang);
            var slangRecords = await _fileHelper.TraverseSlangRecordFileAsync(result, corpusTypeId);
            var slangRecordsResult = await _slangRecordsService.AddSlangRecordAsync(slangRecords, corpusTypeId, ConnectionString);
            if (slangRecordsResult)
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
            var result = await _slangRecordsService.AddSlangRecordAsync(slangRecordModels, slangRecordViewModels.FirstOrDefault().CorpusTypeId, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //DELETE: api/SlangRecord/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlangRecord([FromRoute] int id)
        {
            var result = await _slangRecordsService.DeleteSlangRecordAsync(id, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        
    }
}
