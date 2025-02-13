using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestLogging : ControllerBase
    {
        private readonly ILogger<TestLogging> _logger;

        public TestLogging(ILogger<TestLogging> logger)
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
