using Exam1.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exam1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailTicketController : ControllerBase
    {
        public readonly AvailTicketServices _service;
        public readonly ILogger<AvailTicketController> _logger;
        public AvailTicketController(AvailTicketServices service, ILogger<AvailTicketController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/<AvailTicketController>
        [HttpGet("Show All Data")]
        public async Task<IActionResult> Get(
            [FromQuery] string? categoryName,
            [FromQuery] string? ticketCode,
            [FromQuery] string? ticketName,
            [FromQuery] decimal? maxPrice,
            [FromQuery] DateTime? minEventDate,
            [FromQuery] DateTime? maxEventDate,
            [FromQuery] string? orderBy = "ticketCode",
            [FromQuery] string? orderState = "asc"
            )
        {
            var get = await _service.Get(categoryName,ticketCode,ticketName,maxPrice,minEventDate,maxEventDate,orderBy,orderState);
            _logger.LogInformation("Successfully show Available Tickets");
            return Ok(get);
        }

    }
}
