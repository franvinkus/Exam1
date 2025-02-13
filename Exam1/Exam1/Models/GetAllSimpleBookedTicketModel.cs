using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class GetAllSimpleBookedTicketModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        public string bookedTicketCode { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string bookedTicketName { get; set; } = string.Empty;
        [Required]
        public string bookedSeat { get; set; } = string.Empty;
        [Required]
        public string bookedEventDate { get; set; } = string.Empty;
        public int id { get; set; }

    }
}
