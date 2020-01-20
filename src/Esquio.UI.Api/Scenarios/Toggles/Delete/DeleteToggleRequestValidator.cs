using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Toggles.Delete
{
    public class DetailsToggleRequestValidator
        : AbstractValidator<DeleteToggleRequest>
    {
        public DetailsToggleRequestValidator()
        {
            RuleFor(x => x.ProductName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200);


            RuleFor(x => x.FeatureName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200);

            this.RuleFor(pr => pr.ToggleType)
                .NotEmpty();

        }
    }
}
