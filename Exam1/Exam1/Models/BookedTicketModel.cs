using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class BookedTicketModel
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string bookedEventDate { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        public string bookedTicketCode { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string bookedTicketName { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string bookedCategoryName { get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue)]
        public int bookedQuantity { get; set; }
        [Required]
        public string bookedSeat { get; set; } = string.Empty;
        [Required]
        public int bookedPrice { get; set; }
    }
}
