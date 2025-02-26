using Exam1.Models;
using Exam1.Query;
using FluentValidation;

namespace Exam1.Validator
{
    public class PutBookedTicketValidator : AbstractValidator<PutBookedTicketQuery>
    {
        public PutBookedTicketValidator() 
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Id cannot be Empty");

            RuleForEach(x => x.putDTO)
                .SetValidator(new PutBookedTicketDTOValidator());
        }
    }

    public class PutBookedTicketDTOValidator : AbstractValidator<PutBookedTicketDTO>
    {
        public PutBookedTicketDTOValidator()
        {
            RuleFor(x => x.bookedTicketCode)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(5)
                .WithMessage("Ticket Code Lenght has to be between 4 and 5 characters");

            RuleFor(x => x.bookedTicketName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50)
                .WithMessage("Ticket Name Lenght has to be between 5 and 50 characters");

            RuleFor(x => x.quantity)
                .NotEmpty()
                .WithMessage("Quantity Cannot be Empty");

            RuleFor(x => x.bookedCategoryName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50)
                .WithMessage("Category Name lenght has to be between 5 and 50 characters");

        }
    }
}
