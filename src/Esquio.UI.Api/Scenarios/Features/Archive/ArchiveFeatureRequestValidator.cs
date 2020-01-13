using FluentValidation;

namespace Esquio.UI.Api.Features.Flags.Archive
{
    class ArchiveFeatureRequestValidator
        : AbstractValidator<ArchiveFeatureRequest>
    {
        public ArchiveFeatureRequestValidator()
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
