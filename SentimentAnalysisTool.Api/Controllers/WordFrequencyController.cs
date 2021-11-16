﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordFrequencyController : ControllerBase
    {
        private readonly IWordFrequencyService _wordFrequencyService;
        private IConfiguration _configuration;
        private string ConnectionString { get; }
        public WordFrequencyController(IWordFrequencyService wordFrequencyService,IConfiguration configuration)
        {
            _wordFrequencyService = wordFrequencyService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("SentimentDBConnection");
        }
    }
}