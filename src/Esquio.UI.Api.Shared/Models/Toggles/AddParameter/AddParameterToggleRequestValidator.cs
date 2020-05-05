using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Toggles.AddParameter
{
    public class AddParameterToggleRequestValidator
        : AbstractValidator<AddParameterToggleRequest>
    {
        public AddParameterToggleRequestValidator()
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

            this.RuleFor(pr => pr.Name)
                .NotEmpty();

            this.RuleFor(rf => rf.Value)
                .NotEmpty();
        }
    }
}
