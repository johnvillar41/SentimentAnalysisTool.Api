using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Services.Services.Interfaces
{
    public interface IAbbreviationsService
    {
        Task<bool> AddAbbreviationAsync(int recordId,);
    }
}
