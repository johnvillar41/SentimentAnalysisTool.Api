namespace SentimentAnalysisTool.Data.Models
{
    public class SlangRecordModel
    {
        public int SlangRecordId { get; set; }
        public CorpusTypeModel CorpusType { get; set; }
        public string SlangName { get; set; }
    }
}
