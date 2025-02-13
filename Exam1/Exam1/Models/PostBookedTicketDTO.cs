namespace Exam1.Models
{
    public class PostBookedTicketDTO
    {
        public int summaryPrice { get; set; }
        public List<PostBookedCategoryModel> ticketPerCategory { get; set; } = new List<PostBookedCategoryModel>();
    }
}
