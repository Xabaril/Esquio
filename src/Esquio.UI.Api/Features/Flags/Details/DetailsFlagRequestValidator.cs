using FluentValidation;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequestValidator
        : AbstractValidator<DetailsFlagRequest>
    {
        public DetailsFlagRequestValidator()
        {
            this.RuleFor(rf => rf.FeatureName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(ApiConstants.Constraints.NamesRegularExpression);

            this.RuleFor(rf => rf.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(ApiConstants.Constraints.NamesRegularExpression);
        }
    }
}
