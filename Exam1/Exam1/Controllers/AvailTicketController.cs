using Exam1.Query;
using Exam1.Services;
using iText.Kernel.Geom;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exam1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailTicketController : ControllerBase
    {
        //public readonly AvailTicketServices _service;
        public readonly ILogger<AvailTicketController> _logger;
        public readonly IMediator _mediator;
        public AvailTicketController(ILogger<AvailTicketController> logger, IMediator med)
        {
            //_service = service;
            _logger = logger;
            _mediator = med;
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
            [FromQuery] string? orderState = "asc",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
            )
        {

            var query = new GetAvailTicketQuery(categoryName, ticketCode, ticketName, maxPrice, minEventDate, maxEventDate, orderBy, orderState, pageNumber, pageSize);
            var get = await _mediator.Send(query);
            _logger.LogInformation("Successfully show Available Tickets");
            return Ok(get);
        }

    }
}