using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class PutBookedTicketDTO
    {
        [Required]
        [MinLength(4)]
        [MaxLength(5)]
        public string bookedTicketCode { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string bookedTicketName { get; set; } = string.Empty;
        [Required]
        public int quantity { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string bookedCategoryName { get; set; } = string.Empty;
    }
}
