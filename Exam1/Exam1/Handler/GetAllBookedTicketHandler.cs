using Exam1.Models;
using Exam1.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticket.Entities;

namespace Exam1.Handler
{
    public class GetAllBookedTicketHandler : IRequestHandler<GetAllBookedTicketQuery, List<GetAllBookedCategoryModel>>
    {
        public readonly Exam1Context _db;
        public GetAllBookedTicketHandler(Exam1Context db)
        {
            _db = db;
        }

        public async Task<List<GetAllBookedCategoryModel>> Handle(GetAllBookedTicketQuery request, CancellationToken cancellationToken)
        {
            var datas = await _db.BookedTickets
                .GroupBy(bt => bt.CategoryName.Trim())
                .ToListAsync();

            var data = datas
                .Select(Q => new GetAllBookedCategoryModel
                {
                    qtyPerCategory = Q.Sum(bt => bt.Quota),
                    bookedCategoryName = Q.Key,
                    bookedTickets = Q.Select(bt => new GetAllSimpleBookedTicketModel
                    {
                        bookedTicketCode = bt.TicketCode,
                        bookedTicketName = bt.TicketName,
                        bookedSeat = bt.Seat,
                        bookedEventDate = bt.EventDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        id = bt.BookedId
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
