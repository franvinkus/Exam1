namespace Exam1.Models
{
    public class PaginationBookedAllModel<T>
    {
        public int totalPages { get; set; }

        public List<T> bookedTickets { get; set; } = new List<T>();
    }
}
