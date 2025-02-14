using Exam1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;
using Ticket.Entities;

namespace Exam1.Services
{
    public class AvailTicketServices
    {
        public readonly Exam1Context _db;
        public AvailTicketServices(Exam1Context db)
        {
            _db = db;
        }

        public async Task<PaginationModel<AvailTicketModel>> Get(
            string? categoryName,
            string? ticketCode,
            string? ticketName,
            decimal? maxPrice,
            DateTime? minEventDate,
            DateTime? maxEventDate,
            string? orderBy = "ticketCode",
            string? orderState = "asc",
            int pageNumber = 1,
            int pageSize = 10
            )
        {
            //To remove Available Tickets if quota reaches 0
            await RemoveZeroTicket();

            var query = _db.AvailableTickets.AsQueryable();

            //filter Category
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(Q => Q.CategoryName.ToLower().Contains(categoryName.Trim().ToLower()));
            }

            //filter Ticket Code
            if (!string.IsNullOrEmpty(ticketCode))
            {
                query = query.Where(Q => Q.TicketCode.ToLower().Contains(ticketCode.Trim().ToLower()));
            }

            //filter Ticket Name
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(Q => Q.CategoryName.ToLower().Contains(categoryName.Trim().ToLower()));
            }

            //filter Max Price
            if (maxPrice.HasValue)
            {
                query = query.Where(Q => Q.Price <= maxPrice.Value);
            }

            //filter Min EventDate & Max EventDate
            if(minEventDate.HasValue && maxEventDate.HasValue)
            {
                query = query.Where(Q => Q.EventDate >= minEventDate && Q.EventDate <= maxEventDate);
            }
            else if (minEventDate.HasValue)
            {
                query = query.Where(Q => Q.EventDate >= minEventDate);
            }
            else if (maxEventDate.HasValue)
            {
                query = query.Where(Q => Q.EventDate <= maxEventDate);
            }

            query = orderBy.ToLower() switch
            {
                "categoryName" => orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.CategoryName) : query.OrderBy(Q => Q.CategoryName),
                "ticketcode" => orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.TicketCode) : query.OrderBy(Q => Q.TicketCode),
                "ticketname" => orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.TicketName) : query.OrderBy(Q => Q.TicketName),
                "price" => orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.Price) : query.OrderBy(Q => Q.Price),
                "eventdate" => orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.EventDate) : query.OrderBy(Q => Q.EventDate),
                _ => query.OrderBy(Q => Q.TicketCode)
            };

            var count = await query.CountAsync();

            var datas = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(Q => new AvailTicketModel
                {
                    eventDate = Q.EventDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    ticketCode = Q.TicketCode,
                    ticketName = Q.TicketName,
                    categoryName = Q.CategoryName,
                    seat = Q.Seat,
                    quota = Q.Quota,
                    price = Q.Price
                }).ToListAsync();

            return new PaginationModel<AvailTicketModel>
            {
                tickets = datas,
                totalTickets = datas.Count,
            };
        }


        //Menghapus data pada BookedTicket dan AvailTicket jika data quota kedua tabel = 0
        public async Task RemoveZeroTicket()
        {
            var deleteWaste = await _db.AvailableTickets
                .Where(Q => Q.Quota == 0)
                .Select(Q => Q.TicketCode)
                .ToListAsync();

            if (deleteWaste.Any())
            {

                var bookedTicketQuantity = await _db.BookedTickets
                    .Where(Q => deleteWaste.Contains(Q.TicketCode) && Q.Quota == 0)
                    .ToListAsync();

                if (bookedTicketQuantity.Any())
                {
                    _db.BookedTickets.RemoveRange(bookedTicketQuantity);
                    await _db.SaveChangesAsync();
                }

                var remaininBookedTickets = await _db.BookedTickets
                    .Where(Q => deleteWaste.Contains(Q.TicketCode))
                    .AnyAsync();

                if (!remaininBookedTickets)
                {
                    _db.AvailableTickets.RemoveRange(
                        _db.AvailableTickets.Where(Q => deleteWaste.Contains(Q.TicketCode))                       
                    );
                    await _db.SaveChangesAsync();
                }

            }

        }
    }
}
