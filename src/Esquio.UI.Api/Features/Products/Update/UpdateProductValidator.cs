using FluentValidation;

namespace Esquio.UI.Api.Features.Products.Update
{
    public class UpdateProductValidator
        : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(250);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
