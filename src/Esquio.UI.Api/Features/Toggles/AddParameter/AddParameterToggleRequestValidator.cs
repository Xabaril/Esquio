using FluentValidation;

namespace Esquio.UI.Api.Features.Toggles.AddParameter
{
    public class AddParameterToggleRequestValidator
        : AbstractValidator<AddParameterToggleRequest>
    {
        public AddParameterToggleRequestValidator()
        {
            this.RuleFor(pr => pr.Name)
                .NotEmpty();

            this.RuleFor(rf => rf.Value)
                .NotEmpty();

            this.RuleFor(rf => rf.ClrType)
               .NotEmpty();
        }
    }
}
