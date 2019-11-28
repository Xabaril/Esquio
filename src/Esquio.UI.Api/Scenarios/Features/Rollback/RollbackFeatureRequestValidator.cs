using FluentValidation;

namespace Esquio.UI.Api.Features.Flags.Rollback
{
    class RollbackFeatureRequestValidator
        : AbstractValidator<RollbackFlagRequest>
    {
        public RollbackFeatureRequestValidator()
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
