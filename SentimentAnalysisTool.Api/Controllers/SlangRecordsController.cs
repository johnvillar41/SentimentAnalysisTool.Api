using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public SlangRecordsController(ISlangRecordsService slangRecordsService, IConfiguration configuration)
        {
            _slangRecordsService = slangRecordsService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        //POST: api/SlangRecord
        [HttpPost]
        public async Task<IActionResult> AddSlangRecord([FromBody] SlangRecordModel slangRecord)
        {
            if (slangRecord == null)
                return NotFound();

            var result = await _slangRecordsService.AddSlangRecordAsync(slangRecord, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //POST: api/SlangRecord
        [HttpPost]
        public async Task<IActionResult> AddSlangRecord([FromBody] IEnumerable<SlangRecordModel> slangRecords)
        {
            if (slangRecords == null)
                return NotFound();

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
    }
}
