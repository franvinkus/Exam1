using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestLoggingController : ControllerBase
    {
        private readonly ILogger<TestLoggingController> _logger;

        public TestLoggingController(ILogger<TestLoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("ini adalah informasi dari logtester");
            return Ok("logging berhasil");
        }
    }
}
