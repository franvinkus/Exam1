using Exam1.Models;
using Exam1.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticket.Entities;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Exam1.Handler
{
    public class DeleteBookedTicketHandler : IRequestHandler<DeleteBookedTicketQuery, DeleteBookedTicketRemainsListModel>
    {
        public readonly Exam1Context _db;
        public DeleteBookedTicketHandler(Exam1Context db)
        {
            _db = db;
        }
        public async Task<DeleteBookedTicketRemainsListModel> Handle(DeleteBookedTicketQuery request, CancellationToken cancellationToken)
        {
            var response = new DeleteBookedTicketRemainsListModel();

            foreach(var ticket in response.remainModels)
            {
                var existingData = await _db.BookedTickets
                .Where(Q => Q.BookedId == request.id && Q.TicketCode == ticket.ticketCode)
                .FirstOrDefaultAsync();

                if (existingData == null)
                {
                    response.errorMessage = "Ticket/Id salah";
                    return response;

                }

                if (existingData.Quota < ticket.quantity)
                {
                    response.errorMessage = "Quantity over";
                    return response;
                }

                existingData.Quota -= ticket.quantity;

                if (existingData.Quota == 0)
                {
                    _db.BookedTickets.Remove(existingData);
                }

                var ticketRemains = await _db.BookedTickets
                    .Where(Q => Q.BookedId == request.id && Q.Quota > 0)
                    .ToListAsync();

                if (ticketRemains.Count == 0)
                {

                    var data = await _db.BookedTickets
                        .Where(Q => Q.BookedId == request.id)
                        .FirstOrDefaultAsync();

                    if (data != null)
                    {
                        _db.BookedTickets.Remove(data);
                    }
                }
            }

            await _db.SaveChangesAsync();

            var dataRemains = await _db.BookedTickets
                .Where(Q => Q.Quota > 0)
                .ToListAsync();

            var remaining = dataRemains.Select(ticket => new DeleteBookedTicketRemainsModel
            {
                ticketCode = ticket.TicketCode,
                ticketName = ticket.TicketName,
                quantity = ticket.Quota,
                categoryName = ticket.CategoryName
            }).ToList();

            response.errorMessage = "Success";
            response.remainModels = remaining;
            return response;
        }
    }
}
