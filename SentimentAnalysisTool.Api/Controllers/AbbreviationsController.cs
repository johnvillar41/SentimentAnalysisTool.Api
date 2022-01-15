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
    [ApiController]
    public class AbbreviationsController : ControllerBase
    {
        private readonly IAbbreviationsService _abbreviationsService;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IFileHelper _fileHelper;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public AbbreviationsController(
            IAbbreviationsService abbreviationsService,
            ICorpusTypeService corpusTypeService,
            IConfiguration configuration,
            IFileHelper fileHelper)
        {
            _abbreviationsService = abbreviationsService;
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
            _fileHelper = fileHelper;
        }
        [HttpPost("{corpusTypeId}")]
        public async Task<IActionResult> AddAbbreviations(
            [FromForm] IFormFile file,
            [FromRoute] int corpusTypeId)
        {
            var filepath = await _fileHelper.UploadCsvAsync(file, UploadType.Abbreviation);
            var abbreviations = await _fileHelper.TraverseAbbreviationsFileAsync(filepath, corpusTypeId);
            var result = await _abbreviationsService.AddAbbreviationAsync(abbreviations, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddAbbreviations(
            [FromBody] IEnumerable<AbbreviationsViewModel> abbreviations)
        {
            var abbreviationModelTasks = abbreviations.Select(async m => new AbbreviationModel()
            {
                AbbreviationsId = -1,
                CorpusType = await _corpusTypeService.FindCorpusTypeAsync(m.CorpusTypeId, ConnectionString),
                Abbreviation = m.Abbreviation,
                AbbreviationWord = m.AbbreviationWord
            });
            var abbreviationModels = await Task.WhenAll(abbreviationModelTasks);
            var result = await _abbreviationsService.AddAbbreviationAsync(abbreviationModels, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpGet("{corpusTypeId}")]
        public async Task<IActionResult> FetchAbbreviations(int corpusTypeId)
        {
            var abbreviations = await _abbreviationsService.FetchAbbreviationsAsync(corpusTypeId, ConnectionString);
            if (!abbreviations.Any())
                return NoContent();

            return Ok(abbreviations);
        }        
    }
}
