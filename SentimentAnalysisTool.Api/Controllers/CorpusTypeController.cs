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

        [HttpPost]
        public async Task<IActionResult> AddCorpusType([FromBody] CorpusTypeViewModel corpusTypeViewModel)
        {
            if (corpusTypeViewModel == null)
                return NotFound();

            var corpusModel = new CorpusTypeModel()
            {
                CorpusTypeId = -1, // Assigning -1 due to adding a new corpusType
                Record = null, //TODO null atm
                CorpusTypeName = corpusTypeViewModel.CorpusTypeName,
                CorpusWords = (IEnumerable<CorpusWordModel>) corpusTypeViewModel.CorpusWordViewModels.Select(async x => new CorpusWordModel()
                {
                    CorpusType = await _corpusTypeService.FindCorpusAsync(x.CorpusTypeId),
                    CorpusWord = x.CorpusWord
                })
            };
            var result = await _corpusTypeService.AddCorpusTypeAsync(corpusModel, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
