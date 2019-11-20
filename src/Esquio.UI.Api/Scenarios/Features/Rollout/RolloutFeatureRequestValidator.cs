using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.Rollout
{
    class RolloutFeatureRequestValidator
        : AbstractValidator<RolloutFeatureRequest>
    {
        public RolloutFeatureRequestValidator()
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
