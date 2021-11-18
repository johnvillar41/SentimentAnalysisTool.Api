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
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public CorpusTypeController(ICorpusTypeService corpusTypeService, IConfiguration configuration)
        {
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        //POST: api/AddCorpusType
        [HttpPost]
        public async Task<IActionResult> AddCorpusType([FromBody] CorpusTypeViewModel corpusTypeViewModel)
        {
            if (corpusTypeViewModel == null)
                return NotFound();

            var corpusModel = new CorpusTypeModel()
            {
                CorpusTypeId = -1,              
                CorpusTypeName = corpusTypeViewModel.CorpusTypeName,          
            };           
            var result = await _corpusTypeService.AddCorpusTypeAsync(corpusModel, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //DELETE: api/DeleteCorpusType/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteCorpusType(int id)
        {
            var result = await _corpusTypeService.DeleteCorpusTypeAsync(id, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
