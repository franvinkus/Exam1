using Exam1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfGeneratorController : ControllerBase
    {
        public readonly PdfGerenatorService _gerenatorService;
        private readonly ILogger<PdfGeneratorController> _logger;
        public PdfGeneratorController(PdfGerenatorService service, ILogger<PdfGeneratorController> logger)
        {
            _gerenatorService = service;
            _logger = logger;
        }

        [HttpGet("PDF-FileReport-AvailableTickets")]
        public async Task<IActionResult> GetPDFReportAvailTicket()
        {
            var availTickets = await _gerenatorService.GetAvailTicket();
            string userDownloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Ensure the directory exists
            if (!Directory.Exists(userDownloadsPath))
            {
                Directory.CreateDirectory(userDownloadsPath);
            }

            // Combine the Downloads directory path with the desired file name
            string filepath = Path.Combine(userDownloadsPath, "AvailableTicketsReport.pdf");

            _gerenatorService.GenerateReportAvailTicket(availTickets, filepath);

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            _logger.LogInformation("Successfully convert from Json to PDF");
            return File(fileBytes, "application/pdf", "BookedTicketsReport.pdf");
        }

        [HttpGet("PDF-FileReport-BookedTicket")]
        public async Task<IActionResult> GetPDFReportBookedTicket()
        {
            var bookedTicket = await _gerenatorService.GetBookedTicket();
            string userDownloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Ensure the directory exists
            if (!Directory.Exists(userDownloadsPath))
            {
                Directory.CreateDirectory(userDownloadsPath);
            }

            // Combine the Downloads directory path with the desired file name
            string filepath = Path.Combine(userDownloadsPath, "BookedTicketsReport.pdf");

            _gerenatorService.GenerateReportBookedTicket(bookedTicket, filepath);

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            _logger.LogInformation("Successfully convert from Json to PDF");
            return File(fileBytes, "application/pdf", "BookedTicketsReport.pdf");
        }
    }
}
