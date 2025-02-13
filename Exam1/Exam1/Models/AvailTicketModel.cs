using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class AvailTicketModel
    {
        [Required]
        public string eventDate {  get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue)]
        public int quota { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        public string ticketCode { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string ticketName { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string categoryName { get; set; } = string.Empty ;
        [Required]
        public string seat { get; set; } = string.Empty;
        [Required]
        public int price { get; set; }

    }
}
