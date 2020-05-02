using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Products.Details
{
    public class DetailsProductRequestValidator
        : AbstractValidator<DetailsProductRequest>
    {
        public DetailsProductRequestValidator()
        {
            this.RuleFor(rf => rf.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);
        }
    }
}
