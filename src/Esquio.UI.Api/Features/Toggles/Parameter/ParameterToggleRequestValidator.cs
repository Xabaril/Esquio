using Esquio.UI.Api.Features.Toggles.Parameter;
using FluentValidation;

namespace Esquio.UI.Api.Features.Flags.List
{
    public class ParameterToggleRequestValidator
        : AbstractValidator<ParameterToggleRequest>
    {
        public ParameterToggleRequestValidator()
        {
            this.RuleFor(pr => pr.Name)
                .NotEmpty();

            this.RuleFor(rf => rf.Value)
                .NotEmpty();

            this.RuleFor(rf => rf.ToogleId)
               .GreaterThan(0);
        }
    }
}
