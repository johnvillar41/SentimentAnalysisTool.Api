namespace SentimentAnalysisTool.Data.Models
{
    public class SlangRecordModel
    {
        public int SlangRecordsId { get; set; }
        public CorpusTypeModel CorpusType { get; set; }
        public string SlangName { get; set; }
    }
}
