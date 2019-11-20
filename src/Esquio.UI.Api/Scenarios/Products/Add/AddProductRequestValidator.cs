using FluentValidation;

namespace Esquio.UI.Api.Features.Products.Add
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
        }
    }
}
