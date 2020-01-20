using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Toggles.AddParameter
{
    public class DeleteToggleRequestValidator
        : AbstractValidator<AddParameterToggleRequest>
    {
        public DeleteToggleRequestValidator()
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

            this.RuleFor(pr => pr.Name)
                .NotEmpty();

            this.RuleFor(rf => rf.Value)
                .NotEmpty();
        }
    }
}
