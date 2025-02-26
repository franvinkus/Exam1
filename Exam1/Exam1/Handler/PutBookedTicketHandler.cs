using Exam1.Models;
using Exam1.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticket.Entities;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Exam1.Handler
{
    public class PutBookedTicketHandler : IRequestHandler<PutBookedTicketQuery, PutBookedTicketRemainsListModel>
    {
        public readonly Exam1Context _db;
        public PutBookedTicketHandler(Exam1Context db)
        {
            _db = db;
        }
        public async Task<PutBookedTicketRemainsListModel> Handle(PutBookedTicketQuery request, CancellationToken cancellationToken)
        {
            var dto = new PutBookedTicketRemainsListModel();

            foreach(var ticket in request.putDTO)
            {
                var isIdExist = await _db.BookedTickets
                .Where(Q => Q.BookedId == request.id)
                .FirstOrDefaultAsync();

                if (isIdExist == null)
                {
                    dto.message = "Id tidak ada";
                    return dto;
                }

                var isCodeExist = await _db.BookedTickets
                    .Where(Q => Q.TicketCode == ticket.bookedTicketCode)
                    .FirstOrDefaultAsync();

                if (isCodeExist == null)
                {
                    dto.message = "Code tidak ada";
                    return dto;
                }

                if (ticket.quantity < 1)
                {
                    dto.message = "Quantity minimal satu";
                    return dto;
                }

                //memanipulasi jumlah quantity pada AvailTicket
                int oldQuota = isIdExist.Quota; //bookedTicket
                int newQuota = ticket.quantity; //user input
                int quotaDiff = oldQuota - newQuota; //berapa bedanya

                var availTicket = await _db.AvailableTickets
                    .Where(Q => Q.TicketCode == ticket.bookedTicketCode)
                    .FirstOrDefaultAsync();

                if (availTicket == null || ticket.quantity > availTicket.Quota)
                {
                    dto.message = "Quantity tidak boleh lebih dari sisah";
                    return dto;
                }

                if (newQuota > oldQuota)
                {
                    availTicket.Quota -= Math.Abs(quotaDiff);
                }

                if (newQuota < oldQuota)
                {
                    availTicket.Quota += Math.Abs(quotaDiff);
                }

                isIdExist.TicketCode = ticket.bookedTicketCode;
                isIdExist.Quota = ticket.quantity;
            }
            

            await _db.SaveChangesAsync();

            var updatedDatas = await _db.BookedTickets
                .Select(Q => new PutBookedTicketDTO
                {
                    bookedTicketCode = Q.TicketCode,
                    bookedTicketName = Q.TicketName,
                    quantity = Q.Quota,
                    bookedCategoryName = Q.CategoryName
                }).ToListAsync();

            dto.message = "Success";
            dto.putDTO = updatedDatas;
            Console.WriteLine(dto.message);

            return dto;
        }
    }
}
