using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Features.Rollback
{
    class RollbackFeatureRequestValidator
        : AbstractValidator<RollbackFeatureRequest>
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
