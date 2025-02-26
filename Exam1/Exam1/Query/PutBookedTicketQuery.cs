using Exam1.Models;
using MediatR;

namespace Exam1.Query
{
    public class PutBookedTicketQuery : IRequest<PutBookedTicketRemainsListModel>
    {

        public int id { get; set; }
        public List<PutBookedTicketDTO> putDTO { get; set; } = new List<PutBookedTicketDTO>();
        public string message { get; set; } = string.Empty;

        public PutBookedTicketQuery(int id, List<PutBookedTicketDTO> putDTO)
        {
            this.id = id;
            this.putDTO = putDTO;
        }
    }
}
