using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class PostBookedCategoryModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string bookedCategoryName { get; set; } = string.Empty;
        public int summaryPrice { get; set; }
        public List<PostSimpleBookedTicketModel> bookedTickets { get; set; } =  new List<PostSimpleBookedTicketModel>();
    }
}
