namespace Exam1.Models
{
    public class PaginationModel<T>
    {
        public List<T> tickets { get; set; } = new List<T>();

        public int totalTickets { get; set; }
    }
}
