﻿using System;

namespace SentimentAnalysisTool.Api.Models
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public int CommentScore { get; set; }
        public string CommentDetail { get; set; }
        public DateTime Date { get; set; }
        public int PolarityScore { get; set; }
    }
}