using FluentValidation;

namespace Esquio.UI.Api.Features.Products.Add
{
    public class AddProductValidator
        : AbstractValidator<AddProductRequest>
    {
        public AddProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(250);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
