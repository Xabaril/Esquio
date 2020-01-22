using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Toggles.Add
{
    public class AddToggleRequestValidator
        : AbstractValidator<AddToggleRequest>
    {
        public AddToggleRequestValidator()
        {
            RuleFor(x => x.ProductName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200);

            RuleFor(x => x.FeatureName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200);

            RuleFor(x => x.RingName)
               .MinimumLength(5)
               .MaximumLength(200)
               .When(r=>!string.IsNullOrEmpty(r.RingName));

            this.RuleFor(pr => pr.ToggleType)
                .NotEmpty();
        }
    }
}
