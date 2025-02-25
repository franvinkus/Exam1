using System.ComponentModel.DataAnnotations;
using Exam1.Models;
using MediatR;

namespace Exam1.Query
{
    public class GetBookedTicketByIdQuery : IRequest<List<GetBookedCategoryModel>>
    {
        public int id { get; set; }
        public int qtyPerCategory { get; set; }
        public string bookedCategoryName { get; set; } = string.Empty;
        public List<GetSimpleBookedTicketModel> bookedTickets { get; set; } = new List<GetSimpleBookedTicketModel>();

        public GetBookedTicketByIdQuery(int id)
        {
            this.id = id;
        }
    }
}
