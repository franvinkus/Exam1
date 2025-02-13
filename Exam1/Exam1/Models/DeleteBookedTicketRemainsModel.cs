namespace Exam1.Models
{
    public class DeleteBookedTicketRemainsModel
    {
        public string ticketCode { get; set; } = string.Empty;
        public string ticketName {  get; set; } = string.Empty;
        public int quantity { get; set; }
        public string categoryName {  get; set; } = string.Empty;
    }
}
