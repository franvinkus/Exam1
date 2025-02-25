using Exam1.Models;
using Exam1.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticket.Entities;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Exam1.Handler
{
    public class GetBookedTicketByIdHandler : IRequestHandler<GetBookedTicketByIdQuery, List<GetBookedCategoryModel>>
    {
        public readonly Exam1Context _db;
        public GetBookedTicketByIdHandler(Exam1Context db)
        {
            _db = db;
        }

        public async Task<List<GetBookedCategoryModel>> Handle(GetBookedTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var datas = await _db.BookedTickets
                .Where(bt => bt.BookedId == request.id)
                .GroupBy(bt => bt.CategoryName.Trim())
                .ToListAsync();

            var data = datas
                .Select(Q => new GetBookedCategoryModel
                {
                    qtyPerCategory = Q.Sum(bt => bt.Quota),
                    bookedCategoryName = Q.Key,
                    bookedTickets = Q.Select(bt => new GetSimpleBookedTicketModel
                    {
                        bookedTicketCode = bt.TicketCode,
                        bookedTicketName = bt.TicketName,
                        bookedSeat = bt.Seat,
                        bookedEventDate = bt.EventDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    }).ToList()
                }).ToList();

            if (data == null || !data.Any())
            {
                return null;
            }

            return data;
        }
    }
}
