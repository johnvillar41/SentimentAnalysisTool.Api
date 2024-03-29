﻿using System;
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
        public const string SP_DELETE_ABBREVIATION = "DeleteAbbreviation";

        public const string SP_FETCH_CORPUS_TYPE = "FetchCorpusType";
        public const string SP_FETCH_CORPUS_WORD = "FetchCorpusWord";
        public const string SP_FETCH_RECORDS = "FetchRecords";
        public const string SP_FETCH_CORPUS_RECORD = "FetchCorpusRecord"; 
        public const string SP_FETCH_WORD_FREQUENCIES = "FetchWordFrequencies";
        public const string SP_FETCH_SLANG_RECORD = "FetchSlangRecord";
        public const string SP_FETCH_SLANG_RECORDS = "FetchSlangRecords";
        public const string SP_FETCH_ABBREVIATION = "FetchAbbreviation";
        public const string SP_FETCH_SYNONYMOUD_WORD = "FetchSynonymousWord";
        public const string SP_FETCH_ABBREVIATIONS = "FetchAbbreviations";
        public const string SP_FETCH_CORPUSTYPES = "FetchCorpustypes";

        public const string SP_PAGINATE_COMMENTS = "PaginateCommentsTable";

        public const string SP_SAVE_COMMENTS = "SaveComments";
        public const string SP_SAVE_CORPUS_RECORDS = "SaveCorpusRecords";
        public const string SP_SAVE_CORPUS_TYPE = "SaveCorpusType";
        public const string SP_SAVE_CORPUS_WORD = "SaveCorpusWord";
        public const string SP_SAVE_RECORDS = "SaveRecords";
        public const string SP_SAVE_SLANG_RECORD= "SaveSlangRecord";
        public const string SP_SAVE_WORD_FREQUENCY = "SaveWordFrequency";
        public const string SP_SAVE_ABBREVIATION = "SaveAbbreviation";
    }
}
