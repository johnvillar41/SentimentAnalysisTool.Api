﻿using Microsoft.AspNetCore.Http;
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
        //GET: api/Comment/3012/10/1
        [HttpGet("{recordId}/{pageSize}/{pageNumber}")]
        public async Task<IActionResult> FetchComments(
            [FromRoute] int recordId = 0,
            [FromRoute] int pageSize = 10,
            [FromRoute] int pageNumber = 1)
        {
            if (recordId == 0)
                return NotFound("No records found");

            var comments = await _commentService.FetchCommentsAsync(pageSize, pageNumber, recordId, ConnectionString);
            if (comments.Count == 0)
                return NotFound("No Comments Found!");

            return Ok(comments);
        }       
    }
}
