using SentimentAnalysisTool.Data.Models;
using System;
using System.Text.Json.Serialization;

namespace SentimentAnalysisTool.Api.Models
{
    public class CommentViewModel<T>
    {
        [JsonPropertyName("commentId")]
        public int CommentId { get; set; }

        [JsonPropertyName("recordId")]
        public int RecordId { get; set; }

        [JsonPropertyName("commentScore")]
        public int CommentScore { get; set; }

        [JsonPropertyName("commentPolarity")]
        public string CommentPolarity { get; set; }

        [JsonPropertyName("commentDetail")]
        public string CommentDetail { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("algorithmnObject")]
        public T AlgorithmnModel { get; set; }

        [JsonPropertyName("transformedCommentDetail")]
        public string TransformedComment { get; set; }
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
