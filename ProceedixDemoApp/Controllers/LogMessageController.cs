using Microsoft.AspNetCore.Mvc;
using ProceedixDemoApp.DTOs;
using ProceedixDemoApp.Services;

namespace ProceedixDemoApp.Controllers
{
    [ApiController]
    [Route("logs")]
    public class LogMessageController : ControllerBase
    {
        private readonly IBackgroundQueue<LogMessageDto[]> _queue;

        public LogMessageController(IBackgroundQueue<LogMessageDto[]> queue)
        {
            _queue = queue;
        }

        /// <summary>Log messages/summary>
        /// <param name="logMessages">A set of log messages</param>
        /// <response code="200">Logs were succesfully queued.</response>
        [HttpPost]
        public IActionResult PostLogMessages([FromBody] LogMessageDto[] logMessages)
        {
            _queue.Enqueue(logMessages);

            return Ok();
        }
    }
}