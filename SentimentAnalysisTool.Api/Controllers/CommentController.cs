using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api.Controllers
{
    public class CommentController : ControllerBase
    {        
        public CommentController()
        {

        }
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile csvFormFile)
        {
            var filePath = Path.GetTempFileName();
            if (csvFormFile.Length > 0)
            {
                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await csvFormFile.CopyToAsync(fileStream);
                return Ok();
            }
            return BadRequest();
        }
    }
}
