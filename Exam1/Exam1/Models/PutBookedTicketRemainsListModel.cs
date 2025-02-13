namespace Exam1.Models
{
    public class PutBookedTicketRemainsListModel
    {
        public List<PutBookedTicketDTO> putDTO { get; set; } = new List<PutBookedTicketDTO>();
        public string message { get; set; } = string.Empty;
    }
}
