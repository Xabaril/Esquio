using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Products.Add
{
    public class AddProductRequestValidator
        : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(ApiConstants.Constraints.NamesRegularExpression);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.DefaultRingName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(200);
        }
    }
}
