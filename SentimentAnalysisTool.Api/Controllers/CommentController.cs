﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Data.Models;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; set; }
        public CommentController(ICommentService commentService, IConfiguration configuration)
        {
            _commentService = commentService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
        //GET: api/Comment/10/1
        [HttpGet("{pageSize}/{pageNumber}")]
        public async Task<IActionResult> FetchComments(int pageSize = 10, int pageNumber = 1)
        {
            var comments = await _commentService.FetchCommentsAsync(pageSize, pageNumber, ConnectionString);
            if (((List<CommentModel>)comments).Count == 0)
                return NoContent();

            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> SaveComments()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile csvFormFile)
        {
            var filePath = Path.GetTempFileName();
            if (csvFormFile.Length > 0)
            {
                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await csvFormFile.CopyToAsync(fileStream);
                return Ok();
            }
            return BadRequest();
        }
    }
}
