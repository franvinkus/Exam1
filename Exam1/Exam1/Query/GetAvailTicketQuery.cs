using Exam1.Models;
using MediatR;

namespace Exam1.Query
{
    public class GetAvailTicketQuery : IRequest<PaginationModel<AvailTicketModel>>
    {

        public string? categoryName { get; set; }
        public string? ticketCode { get; set; }
        public string? ticketName { get; set; }
        public decimal? maxPrice { get; set; }
        public DateTime? minEventDate { get; set; }
        public DateTime? maxEventDate { get; set; }
        public string? orderBy { get; set; } = "ticketCode";
        public string? orderState { get; set; } = "asc";
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 1;

        public GetAvailTicketQuery(string? categoryName, string? ticketCode, string? ticketName, decimal? maxPrice, DateTime? minEventDate, DateTime? maxEventDate, string? orderBy, string? orderState, int pageNumber, int pageSize)
        {
            this.categoryName = categoryName;
            this.ticketCode = ticketCode;
            this.ticketName = ticketName;
            this.maxPrice = maxPrice;
            this.minEventDate = minEventDate;
            this.maxEventDate = maxEventDate;
            this.orderBy = orderBy;
            this.orderState = orderState;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }

       
    }
}
