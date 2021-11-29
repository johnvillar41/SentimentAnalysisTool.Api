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
    public class WordFrequencyController : ControllerBase
    {
        private readonly IWordFrequencyService _wordFrequencyService;
        private readonly IRecordService _recordService;
        private IConfiguration _configuration;
        private string ConnectionString { get; }
        public WordFrequencyController(IWordFrequencyService wordFrequencyService, IConfiguration configuration, IRecordService recordService)
        {
            _wordFrequencyService = wordFrequencyService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
            _recordService = recordService;
        }
        //POST: api/WordFrequency
        [HttpPost]
        public async Task<IActionResult> AddWordFrequency([FromBody] WordFrequencyViewModel wordFrequencyViewModel)
        {
            var wordFrequencyModel = new WordFrequencyModel()
            {
                Record = await _recordService.FindRecordAsync(wordFrequencyViewModel.RecordId, ConnectionString),
                Word = wordFrequencyViewModel.Word,
                WordFrequency = wordFrequencyViewModel.WordFrequency
            };
            var result = await _wordFrequencyService.AddWordFrequenciesAsync(wordFrequencyModel, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
        //POST: api/WordFrequency
        [HttpPost]
        public async Task<IActionResult> AddWordFrequencies([FromBody] IEnumerable<WordFrequencyViewModel> wordFrequencyViewModels)
        {
            var wordFrequencyModelTasks = wordFrequencyViewModels.Select(async m => new WordFrequencyModel()
            {
                Record = await _recordService.FindRecordAsync(m.RecordId, ConnectionString),
                Word = m.Word,
                WordFrequency = m.WordFrequency
            });
            var wordFrequencyModels = await Task.WhenAll(wordFrequencyModelTasks);
            var result = await _wordFrequencyService.AddWordFrequenciesAsync(wordFrequencyModels, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
