using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Models;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApikeyAuth]
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
        //POST: api/Records
        [HttpPost]
        public async Task<IActionResult> AddRecord([FromBody] RecordViewModel recordViewModel)
        {
            if (recordViewModel == null)
                return NotFound();

            
            var recordModel = new RecordModel()
            {
                RecordName = recordViewModel.RecordName,
                PositivePercent = recordViewModel.PositivePercent,
                NegativePercent = recordViewModel.NegativePercent
            };

            //Initialization of transaction and connection
            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            //Beginning of database transaction
            //Insertion for RecordsTable
            var resultPrimaryKey = await _recordService.AddRecordAsync(recordModel, transaction, connection);
            recordModel.RecordId = resultPrimaryKey;
            if (recordModel.RecordId < 1)
                return BadRequest("Error Adding Record!");
            
            //Insertion for CommentsTable
            var commentModels = recordViewModel.CommentViewModels.Select(x => new CommentModel()
            {
                CommentId = x.CommentId,
                Record = recordModel,
                CommentScore = x.CommentScore,
                CommentDetail = x.CommentDetail,
                Date = x.Date
            });
            var commentsResult = await _commentService.SaveCommentsAsync(commentModels, transaction, connection);
            if (!commentsResult)
                return BadRequest("Error Adding Comments!");

            //Insertion for CorpusRecordsTable
            var corpusRecordModels = recordViewModel.CorpusRecordViewModels.Select(x => new CorpusRecordModel()
            {
                CorpusRecordId = -1,
                Record = recordModel,
                CorpusType = new CorpusTypeModel()
                {
                    CorpusTypeId = x.CorpusTypeId,
                    CorpusTypeName = _corpusTypeService.FindCorpusTypeAsync(x.CorpusTypeId, ConnectionString).Result.CorpusTypeName,
                    CorpusWords = new List<CorpusWordModel>()
                }
            });
            var corpusRecordServiceResult = await _corpusRecordService.AddCorpusRecordAsync(corpusRecordModels, transaction, connection);
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
            var wordFrequencyServiceResult = await _wordFrequencyService.AddWordFrequenciesAsync(wordFrequencyModels, transaction, connection);
            if (!wordFrequencyServiceResult)
                return BadRequest("Error Adding WordFrequencies!");

            //Commitment to database transaction
            await transaction.CommitAsync();
            return Ok();
        }
        //DELETE: api/Records/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var result = await _recordService.DeleteRecordAsync(id, ConnectionString);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
