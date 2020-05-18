using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Toggles.Add
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

            RuleFor(x => x.DeploymentName)
               .MinimumLength(5)
               .MaximumLength(200)
               .When(r=>!string.IsNullOrEmpty(r.DeploymentName));

            this.RuleFor(pr => pr.ToggleType)
                .NotEmpty();
        }
    }
}
