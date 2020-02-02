using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Store.Details
{
    public class DetailsStoreRequestValidator
        : AbstractValidator<DetailsStoreRequest>
    {
        public DetailsStoreRequestValidator()
        {
            RuleFor(x => x.ProductName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200)
               .Matches(ApiConstants.Constraints.NamesRegularExpression);

            RuleFor(x => x.FeatureName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200)
               .Matches(ApiConstants.Constraints.NamesRegularExpression);
        }
    }
}
