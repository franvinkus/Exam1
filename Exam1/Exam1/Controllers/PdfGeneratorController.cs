﻿using Exam1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfGeneratorController : ControllerBase
    {
        public readonly PdfGerenatorService _gerenatorService;
        public PdfGeneratorController(PdfGerenatorService service)
        {
            _gerenatorService = service;
        }

        [HttpGet("PDF-FileReport")]
        public async Task<IActionResult> GetPDFReport()
        {
            var bookedTicket = await _gerenatorService.getBookedTicket();
            string userDownloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Ensure the directory exists
            if (!Directory.Exists(userDownloadsPath))
            {
                Directory.CreateDirectory(userDownloadsPath);
            }

            // Combine the Downloads directory path with the desired file name
            string filepath = Path.Combine(userDownloadsPath, "BookedTicketsReport.pdf");

            _gerenatorService.GenerateReport(bookedTicket, filepath);

            var fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/pdf", "BookedTicketsReport.pdf");
        }
    }
}
