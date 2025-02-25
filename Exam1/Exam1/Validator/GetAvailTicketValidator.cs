using Exam1.Query;
using FluentValidation;
using iText.Layout.Element;

namespace Exam1.Validator
{
    public class GetAvailTicketValidator : AbstractValidator<GetAvailTicketQuery>
    {
        public GetAvailTicketValidator()
        {
            RuleFor(x => x.pageNumber)
                .GreaterThan(0).WithMessage("Page Number has to be greater than 1");

            RuleFor(x => x.pageSize)
                .GreaterThan(0).WithMessage("Page Size cannot be 0")
                .LessThan(100).WithMessage("Page Size cannot be greater than 100");

            RuleFor(x => x.orderState)
                .Must(state => state.ToLower() == "asc" || state.ToLower() == "desc").
                WithMessage("Order state can either be asc or desc");

            RuleFor(x => x.maxPrice)
                .GreaterThan(0).When(x => x.maxPrice.HasValue)
                .WithMessage("Max price has to be greater than 0");

            RuleFor(x => x)
                .Must(x => x.minEventDate == null || x.maxEventDate == null || x.minEventDate <= x.maxEventDate)
                .WithMessage("Min Event Date can't be lesser than Max Event Date");
        }
    }
}
