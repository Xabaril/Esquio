using FluentValidation;

namespace Esquio.UI.Api.Features.Products.Delete
{
    public class DeleteProductRequestValidator
        : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(ApiConstants.Constraints.NamesRegularExpression);
        }
    }
}
