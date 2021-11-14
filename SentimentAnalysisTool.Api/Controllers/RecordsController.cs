using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ICorpusRecordService _corpusRecordService;
        private readonly IWordFrequencyService _wordFrequencyService;
        private readonly IRecordService _recordService;
        private readonly ICorpusTypeService _corpusTypeService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public RecordsController(
            ICommentService commentService,
            ICorpusRecordService corpusRecordService,
            IWordFrequencyService wordFrequencyService,
            IRecordService recordService,
            ICorpusTypeService corpusTypeService,
            IConfiguration configuration)
        {
            _commentService = commentService;
            _corpusRecordService = corpusRecordService;
            _wordFrequencyService = wordFrequencyService;
            _recordService = recordService;
            _corpusTypeService = corpusTypeService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        [HttpPost]
        public async Task<IActionResult> AddRecord([FromBody] RecordViewModel recordViewModel)
        {
            if (recordViewModel == null)
                return NotFound();

            //Insertion for RecordsTable
            var recordModel = new RecordModel()
            {                
                RecordName = recordViewModel.RecordName,
                PositivePercent = recordViewModel.PositivePercent,
                NegativePercent = recordViewModel.NegativePercent
            };
            var resultPrimaryKey = await _recordService.AddRecordAsync(recordModel, ConnectionString);
            if (resultPrimaryKey < 1)
                return BadRequest("Error Adding Record!");
            recordModel.RecordId = resultPrimaryKey;

            //Insertion for CommentsTable
            var commentModelTasks = recordViewModel.CommentViewModels.Select(async x => new CommentModel()
            {
                CommentId = x.CommentId,
                Record = recordModel,
                CommentScore = x.CommentScore,
                CommentDetail = x.CommentDetail,
                Date = x.Date
            });
            var commentModels = await Task.WhenAll(commentModelTasks);
            var commentsResult = await _commentService.SaveCommentsAsync(commentModels, ConnectionString);
            if (!commentsResult)
                return BadRequest("Error Adding Comments!");

            //Insertion for CorpusRecordsTable
            var corpusRecordModels = recordViewModel.CorpusRecordViewModels.Select(x => new CorpusRecordModel()
            {
                CorpusRecordId = -1,
                Record = recordModel,
                CorpusName = x.CorpusName,
                CorpusTypes = x.CorpusTypeIds.Select(y => new CorpusTypeModel()
                {
                    CorpusTypeId = -1,
                    Record = recordModel,
                    CorpusTypeName = _corpusTypeService.FindCorpusTypeAsync(y, ConnectionString).Result.CorpusTypeName,
                    CorpusWords = new List<CorpusWordModel>()
                })
            });            
            var corpusRecordServiceResult = await _corpusRecordService.AddCorpusRecordAsync(corpusRecordModels, ConnectionString);
            if (!corpusRecordServiceResult)
                return BadRequest("Error Adding Corpus!");

            //Insertion for WordFrequencyTable
            var wordFrequencyModels = recordViewModel.WordFrequencyViewModels.Select(x => new WordFrequencyModel()
            {
                WordFrequencyId = -1,
                Record = recordModel,
                Word = x.Word,
                WordFrequency = x.WordFrequency
            });           
            var wordFrequencyServiceResult = await _wordFrequencyService.AddWordFrequenciesAsync(wordFrequencyModels, ConnectionString);
            if (!wordFrequencyServiceResult)
                return BadRequest("Error Adding WordFrequencies");

            return Ok();
        }
    }
}
