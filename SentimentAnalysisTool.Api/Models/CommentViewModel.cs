using Newtonsoft.Json;
using SentimentAnalysisTool.Data.Models;
using System;
using System.Text.Json.Serialization;

namespace SentimentAnalysisTool.Api.Models
{
    public class CommentViewModel<T>
    {
        [JsonProperty("commentId")]
        public int CommentId { get; set; }

        [JsonProperty("recordId")]
        public int RecordId { get; set; }

        [JsonProperty("commentScore")]
        public int CommentScore { get; set; }

        [JsonProperty("commentPolarity")]
        public string CommentPolarity { get; set; }

        [JsonProperty("commentDetail")]
        public string CommentDetail { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("algorithmnObject")]
        public T AlgorithmnModel { get; set; }

        [JsonProperty("transformedCommentDetail")]
        public string TransformedComment { get; set; }

        [JsonProperty("manualTransformedComment")]
        public string ManualTransformedComment { get; set; }
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
