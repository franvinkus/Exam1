namespace Exam1.Models
{
    public class DeleteBookedTicketRemainsListModel
    {
        public List<DeleteBookedTicketRemainsModel> remainModels { get; set; } = new List<DeleteBookedTicketRemainsModel>();
        public string errorMessage {  get; set; } = string.Empty;
    }
}
