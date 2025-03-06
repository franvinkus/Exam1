using System.ComponentModel.DataAnnotations;
using Exam1.Models;
using MediatR;

namespace Exam1.Query
{
    public class GetAllBookedTicketQuery : IRequest<PaginationBookedAllModel<GetAllBookedCategoryModel>>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int qtyPerCategory { get; set; }
        public string bookedCategoryName { get; set; } = string.Empty;
        public List<GetAllSimpleBookedTicketModel> bookedTickets { get; set; } = new List<GetAllSimpleBookedTicketModel>();

        public GetAllBookedTicketQuery(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }

    }
}
