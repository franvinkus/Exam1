using Exam1.Models;
using Exam1.Query;
using FluentValidation;

namespace Exam1.Validator
{
    public class PostBookedTicketValidator : AbstractValidator<PostBookedTicketQuery>
    {
        public PostBookedTicketValidator()
        {
            RuleFor(x => x.summaryPrice)
                .GreaterThan(0)
                .WithMessage("Summary Price has to be more than 0");

            RuleFor(x => x.ticketPerCategory)
                .NotEmpty()
                .WithMessage("Ticket categories cannot be empty");

            RuleForEach(x => x.ticketPerCategory)
                .SetValidator(new PostBookedPerTicketValidator());
        }
    }

    public class PostBookedPerTicketValidator : AbstractValidator<PostBookedCategoryModel>
    {
        public PostBookedPerTicketValidator()
        {
            RuleFor(x => x.summaryPrice)
                .GreaterThan(0)
                .WithMessage("Ticket Code doesn't match / wrong Ticket Code");

            RuleForEach(x => x.bookedTickets)
                .SetValidator(new PostBookedSimpleTicketValidator());
        }
    }

    public class PostBookedSimpleTicketValidator : AbstractValidator<PostSimpleBookedTicketModel>
    {
        public PostBookedSimpleTicketValidator()
        {
            RuleFor(x => x.bookedTicketCode)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(5)
                .WithMessage("Ticket code must be between 4 and 5 characters");

            RuleFor(x => x.bookedTicketName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50)
                .WithMessage("Ticket code must be between 5 and 50 characters");

            RuleFor(t => t.bookedSeat)
                .NotEmpty()
                .WithMessage("Seat information is required");

            RuleFor(t => t.bookedPrice)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(t => t.quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");
        }
    }
}
