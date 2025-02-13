using System;
using System.Collections.Generic;

namespace Ticket.Entities;

public partial class BookedTicket
{
    public int BookedId { get; set; }

    public DateTime EventDate { get; set; }

    public int Quota { get; set; }

    public string? TicketCode { get; set; }

    public string TicketName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string Seat { get; set; } = null!;

    public int Price { get; set; }

    public virtual AvailableTicket? TicketCodeNavigation { get; set; }
}
