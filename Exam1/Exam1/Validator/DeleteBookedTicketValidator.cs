using Exam1.Models;
using Exam1.Query;
using FluentValidation;

namespace Exam1.Validator
{
    public class DeleteBookedTicketValidator : AbstractValidator<DeleteBookedTicketQuery>
    {
        public DeleteBookedTicketValidator() 
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Booked Ticket id Cannot be empty");

            RuleFor(x => x.ticketCode)
                .NotEmpty()
                .WithMessage("Ticket code cannot be empty");

            RuleFor(x => x.quantity)
                .NotEmpty()
                .WithMessage("Quantity Cannot be Empty");
        }
    }
}
