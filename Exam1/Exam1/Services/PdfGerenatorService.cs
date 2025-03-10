﻿using Exam1.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf.Canvas.Draw;
using System.Collections.Generic;
using Ticket.Entities;
using Microsoft.EntityFrameworkCore;
using iText.Layout.Properties;


namespace Exam1.Services
{
    public class PdfGerenatorService
    {
        public readonly Exam1Context _db;

        public PdfGerenatorService(Exam1Context db)
        {
            _db = db;
        }

        public async Task<List<AvailTicketModel>> GetAvailTicket()
        {
            var allAvailTicket = await _db.AvailableTickets
                .Select(Q => new AvailTicketModel
                {
                    ticketCode = Q.TicketCode,
                    ticketName = Q.TicketName,
                    eventDate = Q.EventDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    categoryName = Q.CategoryName,
                    quota = Q.Quota,
                    seat = Q.Seat,
                    price = Q.Price
                }).ToListAsync();

            return allAvailTicket;
        }

        public void GenerateReportAvailTicket(List<AvailTicketModel> model, string filepath)
        {
            using (var writer = new PdfWriter(filepath))
            using (var pdf = new PdfDocument(writer))

            {
                var document = new Document(pdf);

                document.Add(new Paragraph("Booked Ticket Report")
                    .SetFontSize(20)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(20)
                    );

                var table = new Table(7);
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Event Date").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Ticket Code").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Ticket Name").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Category").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Quantity").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Seat").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Price").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                // Loop through the list of tickets and add rows
                foreach (var ticket in model)
                {
                    table.AddCell(ticket.ticketCode)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.ticketName)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetPadding(5);

                    table.AddCell(ticket.eventDate)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.categoryName)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetPadding(5);

                    table.AddCell(ticket.quota.ToString())
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.seat)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.price.ToString())
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);
                }

                document.Add(table);
            }

        }

        public async Task<List<BookedTicketModel>> GetBookedTicket()
        {
            var allBookedTicket = await _db.BookedTickets
                .Select(Q => new BookedTicketModel
                {
                    id = Q.BookedId,
                    bookedEventDate = Q.EventDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    bookedTicketCode = Q.TicketCode,
                    bookedTicketName = Q.TicketName,
                    bookedCategoryName = Q.CategoryName,
                    bookedQuantity = Q.Quota,
                    bookedSeat = Q.Seat,
                    bookedPrice = Q.Price
                }).ToListAsync();

            return allBookedTicket;
        }

        public void GenerateReportBookedTicket(List<BookedTicketModel> model, string filepath)
        {
            using (var writer = new PdfWriter(filepath))
            using (var pdf = new PdfDocument(writer))

            {
                var document = new Document(pdf);

                document.Add(new Paragraph("Booked Ticket Report")
                    .SetFontSize(20)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(20)
                    );

                var table = new Table(8);
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.AddHeaderCell(new Cell().Add(new Paragraph("ID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Event Date").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Ticket Code").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Ticket Name").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Category").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Quantity").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Seat").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Price").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                // Loop through the list of tickets and add rows
                foreach (var ticket in model)
                {
                    table.AddCell(ticket.id.ToString())
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.bookedEventDate)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.bookedTicketCode)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.bookedTicketName)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetPadding(5);

                    table.AddCell(ticket.bookedCategoryName)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetPadding(5);

                    table.AddCell(ticket.bookedQuantity.ToString())
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.bookedSeat)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);

                    table.AddCell(ticket.bookedPrice.ToString())
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetPadding(5);
                }

                document.Add(table);
            }

        }
    }
}