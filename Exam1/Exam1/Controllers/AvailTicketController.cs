using Exam1.Query;
using Exam1.Services;
using FluentValidation;
using iText.Kernel.Geom;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Ticket.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exam1.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AvailTicketController : ControllerBase
    {
        //public readonly AvailTicketServices _service;
        public readonly ILogger<AvailTicketController> _logger;
        public readonly IMediator _mediator;
        public readonly IValidator<GetAvailTicketQuery> _validator;
        public AvailTicketController(ILogger<AvailTicketController> logger, IMediator med, IValidator<GetAvailTicketQuery> validator)
        {
            //_service = service;
            _logger = logger;
            _mediator = med;
            _validator = validator;
        }

        // GET: api/<AvailTicketController>
        [HttpGet("Show-All-Data")]
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
            
            var validation = await _validator.ValidateAsync(query);
            if (!validation.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Data Tidak Sesuai",
                    Detail = $"Masukkan Data yang sesuai",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = validation.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage) }
                });
            }

            var get = await _mediator.Send(query);
            _logger.LogInformation("Successfully show Available Tickets");
            return Ok(get);
        }

    }
}