using Exam1.Models;
using Exam1.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticket.Entities;

namespace Exam1.Handler
{
    public class PostBookedTicketHandler : IRequestHandler<PostBookedTicketQuery, string>
    {
        public readonly Exam1Context _db;
        public PostBookedTicketHandler(Exam1Context db)
        {
            _db = db;
        }
        public async Task<string> Handle(PostBookedTicketQuery request, CancellationToken cancellationToken)
        {
            foreach (var category in request.ticketPerCategory)
            {
                foreach (var ticket in category.bookedTickets)
                {
                    var newData = new BookedTicket
                    {
                        EventDate = DateTime.Now,
                        TicketCode = ticket.bookedTicketCode,
                        TicketName = ticket.bookedTicketName,
                        CategoryName = category.bookedCategoryName,
                        Seat = ticket.bookedSeat,
                        Price = ticket.bookedPrice,
                        Quota = ticket.quantity
                    };

                    _db.BookedTickets.Add(newData);

                    var availTicket = await _db.AvailableTickets
                        .FirstOrDefaultAsync(a => a.TicketCode == ticket.bookedTicketCode);

                    if (availTicket == null)
                    {
                        return null;
                    }

                    if (DateTime.Now >= availTicket.EventDate)
                    {
                        return "Sudah Expired";
                    }

                    if (availTicket.Quota < ticket.quantity)
                    {
                        return "Tidak Cukup";

                    }

                    if (availTicket.Quota == 0)
                    {
                        return "Habis";
                    }
                    availTicket.Quota -= ticket.quantity;
                }
            }

            await _db.SaveChangesAsync(cancellationToken);

            return "success";
        }
    }
}
