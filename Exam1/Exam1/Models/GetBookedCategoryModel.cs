using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class GetBookedCategoryModel
    {
        [Required]
        public int qtyPerCategory { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string bookedCategoryName { get; set; } = string.Empty;
        public List<GetSimpleBookedTicketModel> bookedTickets { get; set; } = new List<GetSimpleBookedTicketModel>();
    }
}
