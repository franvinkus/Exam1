using Exam1.Models;
using Exam1.Query;
using FluentValidation;
using iText.Kernel.Geom;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticket.Entities;


namespace Exam1.Handler
{
    public class GetAvailTicketHandler : IRequestHandler<GetAvailTicketQuery, PaginationModel<AvailTicketModel>>
    {
        public readonly Exam1Context _db;
        public readonly IValidator<GetAvailTicketQuery> _validator;

        public GetAvailTicketHandler(Exam1Context db, IValidator<GetAvailTicketQuery> valid)
        {
            _db = db;
            _validator = valid;
        }

        public async Task<PaginationModel<AvailTicketModel>> Handle(GetAvailTicketQuery request, CancellationToken cancellationToken)
        {

            var validator = await _validator.ValidateAsync(request, cancellationToken);
            if (!validator.IsValid)
            {
                throw new ValidationException(validator.Errors);
            }

            var query = _db.AvailableTickets.AsQueryable();

            //filter Category
            if (!string.IsNullOrEmpty(request.categoryName))
            {
                query = query.Where(Q => Q.CategoryName.ToLower().Contains(request.categoryName.Trim().ToLower()));
            }

            //filter Ticket Code
            if (!string.IsNullOrEmpty(request.ticketCode))
            {
                query = query.Where(Q => Q.TicketCode.ToLower().Contains(request.ticketCode.Trim().ToLower()));
            }

            //filter Ticket Name
            if (!string.IsNullOrEmpty(request.categoryName))
            {
                query = query.Where(Q => Q.CategoryName.ToLower().Contains(request.categoryName.Trim().ToLower()));
            }

            //filter Max Price
            if (request.maxPrice.HasValue)
            {
                query = query.Where(Q => Q.Price <= request.maxPrice.Value);
            }

            //filter Min EventDate & Max EventDate
            if (request.minEventDate.HasValue && request.maxEventDate.HasValue)
            {
                query = query.Where(Q => Q.EventDate >= request.minEventDate && Q.EventDate <= request.maxEventDate);
            }
            else if (request.minEventDate.HasValue)
            {
                query = query.Where(Q => Q.EventDate >= request.minEventDate);
            }
            else if (request.maxEventDate.HasValue)
            {
                query = query.Where(Q => Q.EventDate <= request.maxEventDate);
            }

            query = request.orderBy.ToLower() switch
            {
                "categoryName" => request.orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.CategoryName) : query.OrderBy(Q => Q.CategoryName),
                "ticketcode" => request.orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.TicketCode) : query.OrderBy(Q => Q.TicketCode),
                "ticketname" => request.orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.TicketName) : query.OrderBy(Q => Q.TicketName),
                "price" => request.orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.Price) : query.OrderBy(Q => Q.Price),
                "eventdate" => request.orderState.ToLower() == "desc" ? query.OrderByDescending(Q => Q.EventDate) : query.OrderBy(Q => Q.EventDate),
                _ => query.OrderBy(Q => Q.TicketCode)
            };

            var count = await query.CountAsync();

            var datas = await query
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
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
