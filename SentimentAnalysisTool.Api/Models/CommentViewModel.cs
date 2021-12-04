using SentimentAnalysisTool.Data.Models;
using System;

namespace SentimentAnalysisTool.Api.Models
{
    public class CommentViewModel<T>
    {
        public int CommentId { get; set; }
        public int RecordId { get; set; }
        public int CommentScore { get; set; }
        public string PolarityScore { get; set; }
        public string CommentDetail { get; set; }
        public DateTime Date { get; set; }
        public T AlgorithmnModel { get; set; }
        public CommentViewModel(CommentModel commentModel)
        {
            CommentId = commentModel.CommentId;
            RecordId = commentModel.Record.RecordId;
            CommentScore = commentModel.CommentScore;
            CommentDetail = commentModel.CommentDetail;
            Date = commentModel.Date;            
        }
        public CommentViewModel()
        {

        }
    }
}
