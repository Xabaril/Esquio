using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Features.State
{

    public class StateFeatureRequestValidator
        : AbstractValidator<StateFeatureRequest>
    {
        public StateFeatureRequestValidator()
        {
            this.RuleFor(rf => rf.FeatureName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);

            this.RuleFor(rf => rf.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);
        }
    }
}
