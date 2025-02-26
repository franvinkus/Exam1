using Exam1.Models;
using MediatR;

namespace Exam1.Query
{
    public class DeleteBookedTicketQuery : IRequest<DeleteBookedTicketRemainsListModel>
    {
        public int id { get; set; }
        public string ticketCode { get; set; }
        public int quantity { get; set; }
        public string errorMessage { get; set; } = string.Empty;
        public DeleteBookedTicketQuery(int id, string ticketCode, int quantity)
        {
            this.id = id;
            this.ticketCode = ticketCode;
            this.quantity = quantity;
        }
    }
}
