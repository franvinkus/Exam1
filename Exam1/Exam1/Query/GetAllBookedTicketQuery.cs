using System.ComponentModel.DataAnnotations;
using Exam1.Models;
using MediatR;

namespace Exam1.Query
{
    public class GetAllBookedTicketQuery : IRequest<List<GetAllBookedCategoryModel>>
    {
        public int qtyPerCategory { get; set; }
        public string bookedCategoryName { get; set; } = string.Empty;
        public List<GetAllSimpleBookedTicketModel> bookedTickets { get; set; } = new List<GetAllSimpleBookedTicketModel>();

    }
}
