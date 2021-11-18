using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Api.Helpers;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApikeyAuth]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IFileHelper _fileHelper;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }
        public CommentController(
            ICommentService commentService, 
            IConfiguration configuration,
            IFileHelper fileHelper)
        {
            _commentService = commentService;
            _fileHelper = fileHelper;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        //GET: api/Comment/10/1
        [HttpGet("{pageSize}/{pageNumber}")]
        public async Task<IActionResult> FetchComments(int pageSize = 10, int pageNumber = 1)
        {
            var comments = await _commentService.FetchCommentsAsync(pageSize, pageNumber, ConnectionString);
            if (comments.Count == 0)
                return NotFound("No Comments Found!");

            return Ok(comments);
        }
        //POST: api/SaveComments
        [HttpPost]
        public async Task<IActionResult> SaveComments(IFormFile csvFormFile)
        {
            var isSuccessful = await _fileHelper.UploadCsv(csvFormFile);
            if (!isSuccessful)
                return BadRequest("Error Uploading File!");

            var result = await _fileHelper.PolarizeCsvFile(csvFormFile);
            if(result.Count == 0)
                return BadRequest("Error Polarizing Files!");

            return Ok(result);
        }          
        
    }
}
