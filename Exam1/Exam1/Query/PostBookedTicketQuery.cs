using Exam1.Models;
using MediatR;

namespace Exam1.Query
{
    public class PostBookedTicketQuery : IRequest<String>
    {
        public int summaryPrice { get; set; }
        public List<PostBookedCategoryModel> ticketPerCategory { get; set; } = new List<PostBookedCategoryModel>();
    }
}
