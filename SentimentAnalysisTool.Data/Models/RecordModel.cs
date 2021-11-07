using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysisTool.Data.Models
{
    public class RecordModel
    {
        public int RecordId { get; set; }
        public string RecordName { get; set; }

        public int ReportId { get;set; }
        public ReportModel Report { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; }
        public IEnumerable<CorpusRecordModel> CorpusRecords { get; set; }
    }
}
