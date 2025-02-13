using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class PutSimpleBookedTicketModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(5)]
        public string bookedTicketCode { get; set; } = string.Empty;
        [Required]
        public int quantity { get; set; }
    }
}
