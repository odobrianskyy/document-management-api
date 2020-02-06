using System.Linq;
using DocumentManagement.API.Presentation.Dtos;
using FluentValidation;

namespace DocumentManagement.API.Application.Validation
{
    public class OrderListInputModelValidator : AbstractValidator<OrderListInputModel>
    {
        public OrderListInputModelValidator(int maxOrder)
        {
            RuleFor(x => x.List)
                .NotNull()
                .Must(list => list.GroupBy(x => x.Order).All(g => g.Count() == 1))
                .WithMessage("Cannot set same order for multiple documents")
                .Must(list => list.GroupBy(x => x.Id).All(g => g.Count() == 1))
                .WithMessage("Cannot specify multiple orders for same document")
                .Must(list => list.All(x => x != null));

            RuleForEach(x => x.List)
                .ChildRules(validator =>
                {
                    validator.RuleFor(x => x.Order)
                        .GreaterThan(0)
                        .LessThanOrEqualTo(maxOrder);
                });
        }
    }
}
