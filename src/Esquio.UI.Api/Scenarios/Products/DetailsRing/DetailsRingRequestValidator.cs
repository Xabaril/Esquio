using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Products.DetailsRing
{
    public class DetailsRingRequestValidator
        : AbstractValidator<DetailsRingRequest>
    {
        public DetailsRingRequestValidator()
        {
            this.RuleFor(rf => rf.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);
        }
    }
}
