using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Configuration.Details
{
    public class DetailsConfigurationRequestValidator
        : AbstractValidator<DetailsConfigurationRequest>
    {
        public DetailsConfigurationRequestValidator()
        {
            RuleFor(x => x.ProductName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200)
               .Matches(Constants.Constraints.NamesRegularExpression);

            RuleFor(x => x.FeatureName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200)
               .Matches(Constants.Constraints.NamesRegularExpression);
        }
    }
}
