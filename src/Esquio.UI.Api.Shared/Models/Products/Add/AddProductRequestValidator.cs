using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Products.Add
{
    public class AddProductRequestValidator
        : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(Constants.Constraints.NamesRegularExpression);

            RuleFor(x => x.Description)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(2000);

            RuleFor(x => x.DefaultRingName)
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(200);
        }
    }
}
