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
    public class AbbreviationsController : ControllerBase
    {
        private readonly IAbbreviationsService _abbreviationsService;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public AbbreviationsController(
            IAbbreviationsService abbreviationsService,
            ICorpusTypeService corpusTypeService,
            IConfiguration configuration)
        {
            _abbreviationsService = abbreviationsService;
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        [HttpPost("{recordId}")]
        public async Task<IActionResult> AddAbbreviations(
            [FromHeader] int recordId,
            [FromBody] IEnumerable<AbbreviationsViewModel> abbreviations)
        {
            var abbreviationModelTasks = abbreviations.Select(async m => new AbbreviationModel()
            {
                AbbreviationsId = -1,
                CorpusTypeModel = await _corpusTypeService.FindCorpusTypeAsync(m.CorpusTypeId, ConnectionString),
                Abbreviation = m.Abbreviation,
                AbbreviationWord = m.AbbreviationWord
            });
            var abbreviationModels = await Task.WhenAll(abbreviationModelTasks);
            var result = await _abbreviationsService.AddAbbreviationAsync(recordId, abbreviationModels, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
