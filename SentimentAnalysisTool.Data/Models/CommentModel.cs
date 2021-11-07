using System;
using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysisTool.Data.Models
{
    public class CommentModel
    {
        public int CommentId { get; set; }
        public int CommentScore { get; set; }
        public string CommentDetail { get; set; }
        public DateTime Date { get; set; }

        public RecordModel Record { get; set; }
    }
}
