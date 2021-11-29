using SentimentAnalysisTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Models
{
    public class CorpusRecordViewModel
    {
        public int CorpusRecordsId { get; set; }
        public int RecordId { get; set; }        
        public int CorpusTypeId { get; set; }
        public CorpusRecordViewModel(CorpusRecordModel corpusRecordModel)
        {
            CorpusRecordsId = corpusRecordModel.CorpusRecordId;
            RecordId = corpusRecordModel.Record.RecordId;
            CorpusTypeId = corpusRecordModel.CorpusType.CorpusTypeId;
        }
        public CorpusRecordViewModel()
        {

        }
    }
}
