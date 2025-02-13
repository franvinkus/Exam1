using Exam1.Models;
using Exam1.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exam1.Controllers
{

    [Route("api/v1")]
    [ApiController]
    public class BookedTicketController : ControllerBase
    {
        public readonly BookedTicketServices _service;
        private readonly ILogger<BookedTicketController> _logger;
        public BookedTicketController(BookedTicketServices service, ILogger<BookedTicketController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/<BookedTicketController>
        [HttpGet("Test-Show-All-Data")]
        public async Task<IActionResult> Get()
        {
            var data = await _service.Get1();
            _logger.LogInformation("Successfully show all Booked Ticked");
            return Ok(data);
        }

        // GET api/<BookedTicketController>/5
        [HttpGet("get-booked-ticket/{BookedTicketId}")]
        public async Task<IActionResult> Get(int BookedTicketId)
        {
            var data = await _service.Get(BookedTicketId);

            if (data == null)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Data tidak ditemukan",
                    Detail = $"Tidak ada data ditemukan untuk id {BookedTicketId}",
                    Instance = HttpContext.Request.Path
                };

                return NotFound(problemDetails);
            }
            _logger.LogInformation("Successfully show Booked Ticket by Id");
            return Ok(data);

        }

        // POST api/<BookedTicketController>
        [HttpPost("book-ticket.")]
        public async Task<IActionResult> Post([FromBody] PostBookedTicketDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Data tidak ditemukan",
                    Detail = $"Tidak ditemukan data seperti ini",
                    Instance = HttpContext.Request.Path
                };

                return NotFound(problemDetails);
            }

            try
            {
                var data = await _service.Post(dto);

                if (data == null)
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = 404,
                        Type = "https://httpstatuses.com/404",
                        Title = "Data tidak Ditemukan",
                        Detail = $"TicketCode yang dimasukkan salah",
                        Instance = HttpContext.Request.Path
                    };

                    return NotFound(problemDetails);
                }

                if(data == "Sudah Expired")
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = 404,
                        Type = "https://httpstatuses.com/404",
                        Title = "Expired",
                        Detail = $"Tanggal Event sudah Lewat",
                        Instance = HttpContext.Request.Path
                    };

                    return NotFound(problemDetails);
                }

                if (data == "Habis")
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = 404,
                        Type = "https://httpstatuses.com/404",
                        Title = "Quota tidak cukup",
                        Detail = $"Kuota sudah Habis",
                        Instance = HttpContext.Request.Path
                    };

                    return NotFound(problemDetails);
                }

                if (data == "Tidak Cukup")
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = 404,
                        Type = "https://httpstatuses.com/404",
                        Title = "Quota tidak cukup",
                        Detail = $"Sisa kuota tidak mencukupi total pesanan",
                        Instance = HttpContext.Request.Path
                    };

                    return NotFound(problemDetails);
                }

                //----------------------
                if (data != null)
                {
                    _logger.LogInformation("Successfully Book a Ticket");
                    return CreatedAtAction(nameof(Post), new { summaryPrice = dto.summaryPrice }, dto);
                }

                else
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = 404,
                        Type = "https://httpstatuses.com/404",
                        Title = "Gagal menyimpan Data",
                        Detail = $"TGagal menyimpan Data",
                        Instance = HttpContext.Request.Path
                    };

                    return NotFound(problemDetails);
                }
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        // PUT api/<BookedTicketController>/5
        [HttpPut("edit-booked-ticket/{BookedTicketId}")]
        public async Task<IActionResult> Put(int BookedTicketId, [FromBody] PutSimpleBookedTicketModel model)
        { 
            var update = await _service.Update(BookedTicketId, model);

            if (update.message == "Id tidak ada") 
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Id tidak ada",
                    Detail = $"Tidak ditemukan data dengan Id ini",
                    Instance = HttpContext.Request.Path
                };

                return NotFound(problemDetails);
            }

            if (update.message == "Code tidak ada")
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Code tidak ada",
                    Detail = $"Tidak ditemukan data dengan TicketCode ini",
                    Instance = HttpContext.Request.Path
                };

                return NotFound(problemDetails);
            }

            if (update.message == "Quantity minimal satu")
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Quantity minimal satu",
                    Detail = $"Tidak bisa mengosongkan quantity",
                    Instance = HttpContext.Request.Path
                };

                return NotFound(problemDetails);
            }

            if (update.message == "Quantity tidak boleh lebih dari sisah")
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Quantity tidak boleh lebih dari sisah",
                    Detail = $"Quantity yang diganti harus melebihi dari ticket yang tersedia",
                    Instance = HttpContext.Request.Path
                };

                return NotFound(problemDetails);
            }
            _logger.LogInformation("Successfully Update a Booked Ticket");
            return Ok(update);
        }

        // DELETE api/<BookedTicketController>/5
        [HttpDelete("revoke-ticket/{BookedTicketId}/{KodeTicket}/{Qty}")]
        public async Task<IActionResult> Delete(int BookedTicketId, string KodeTicket, int Qty)
        {
            var delete = await _service.Delete(BookedTicketId, KodeTicket, Qty);


            if(delete.errorMessage == "Ticket/Id salah")
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Data tidak ditemukan",
                    Detail = $"Tidak ditemukan data dengan TicketCode atau Id ini",
                    Instance = HttpContext.Request.Path
                };

                return NotFound(problemDetails);
            }

            if(delete.errorMessage == "Quantity over")
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Type = "https://httpstatuses.com/404",
                    Title = "Quantity Melebihi",
                    Detail = $"Jumlah quantity yang ingin dihapus melebihi",
                    Instance = HttpContext.Request.Path
                };

                return NotFound(problemDetails);
            }
            _logger.LogInformation("Successfully remove Booked Ticket(s)");
            return Ok(delete);
        }
    }
}
