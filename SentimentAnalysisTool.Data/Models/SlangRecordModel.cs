namespace SentimentAnalysisTool.Data.Models
{
    public class SlangRecordModel
    {
        public int SlangRecordId { get; set; }
        public RecordModel Record { get; set; }
        public string SlangName { get; set; }
    }
}
