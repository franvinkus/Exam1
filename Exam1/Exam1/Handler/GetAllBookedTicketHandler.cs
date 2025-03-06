using Exam1.Models;
using Exam1.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticket.Entities;

namespace Exam1.Handler
{
    public class GetAllBookedTicketHandler : IRequestHandler<GetAllBookedTicketQuery, PaginationBookedAllModel<GetAllBookedCategoryModel>>
    {
        public readonly Exam1Context _db;
        public GetAllBookedTicketHandler(Exam1Context db)
        {
            _db = db;
        }

        public async Task<PaginationBookedAllModel<GetAllBookedCategoryModel>> Handle(GetAllBookedTicketQuery request, CancellationToken cancellationToken)
        {

            var query =  _db.BookedTickets.AsQueryable();

            var datas = await _db.BookedTickets
                .GroupBy(bt => bt.CategoryName.Trim())
                .ToListAsync();

            var data = await query
                .OrderBy(Q => Q.CategoryName)
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .GroupBy(bt => bt.CategoryName.Trim())
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
                }).ToListAsync();
            int totalRecords = await query.CountAsync();


            if (data == null || !data.Any())
            {
                return null;
            }

            return new PaginationBookedAllModel<GetAllBookedCategoryModel> { 
                bookedTickets = data,
                totalPages = (int)Math.Ceiling((double)totalRecords / request.pageSize)
            };

        }
    }

}
