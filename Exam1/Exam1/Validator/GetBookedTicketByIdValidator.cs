using Exam1.Query;
using FluentValidation;

namespace Exam1.Validator
{
    public class GetBookedTicketByIdValidator : AbstractValidator<GetBookedTicketByIdQuery>
    {
        public GetBookedTicketByIdValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0)
                .WithMessage("Id Has to be other than 0");
        }
    }
}
