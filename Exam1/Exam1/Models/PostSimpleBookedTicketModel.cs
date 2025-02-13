using System.ComponentModel.DataAnnotations;
using Ticket.Entities;

namespace Exam1.Models
{
    public class PostSimpleBookedTicketModel
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
        public string bookedSeat { get; set; } = string.Empty;
        [Required]
        public int bookedPrice { get; set; }
        [Required]
        public int quantity { get; set; }
    }
}
