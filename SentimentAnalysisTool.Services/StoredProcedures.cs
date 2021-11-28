using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services
{
    public class StoredProcedures
    {
        public const string SP_DELETE_CORPUS_TYPE = "DeleteCorpusType";
        public const string SP_DELETE_CORPUS_WORD = "DeleteCorpusWord";
        public const string SP_DELETE_RECORDS = "DeleteRecords";
        public const string SP_DELETE_SLANG_RECORD = "DeleteSlangRecord";

        public const string SP_FETCH_CORPUS_TYPE = "FetchCorpusType";
        public const string SP_FETCH_CORPUS_WORD = "FetchCorpusWord";
        public const string SP_FETCH_RECORDS = "FetchRecords";
        public const string SP_FETCH_CORPUS_RECORD = "FetchCorpusRecord"; //TODO Build this stored procedure on database
        public const string SP_FETCH_WORD_FREQUENCIES = "FetchWordFrequencies";//TODO Build this stored procedure on database

        public const string SP_PAGINATE_COMMENTS = "PaginateCommentsTable";

        public const string SP_SAVE_COMMENTS = "SaveComments";
        public const string SP_SAVE_CORPUS_RECORDS = "SaveCorpusRecords";
        public const string SP_SAVE_CORPUS_TYPE = "SaveCorpusType";
        public const string SP_SAVE_CORPUS_WORD = "SaveCorpusWord";
        public const string SP_SAVE_RECORDS = "SaveRecords";
        public const string SP_SAVE_SLANG_RECORD= "SaveSlangRecord";
        public const string SP_SAVE_WORD_FREQUENCY = "SaveWordFrequency";        
    }
}
