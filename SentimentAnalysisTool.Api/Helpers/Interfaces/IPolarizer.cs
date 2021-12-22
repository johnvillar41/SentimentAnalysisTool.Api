using SentimentAnalysisTool.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Helpers.Interfaces
{
    public interface IPolarizer
    {
        Task<RecordViewModel<T>> PolarizeCsvFileAsync<T>(PolarizeCsvFileViewModel polarizeCsvFileViewModel);
    }
}
