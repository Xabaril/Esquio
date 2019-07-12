using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.Update
{
    public class UpdateFlagRequestValidator
        : AbstractValidator<UpdateFlagRequest>
    {
        public UpdateFlagRequestValidator()
        {
            this.RuleFor(f=>f.FlagId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);

            this.RuleFor(f => f.Name)
                .MaximumLength(200)
                .NotNull();

            this.RuleFor(f => f.Description)
                .MaximumLength(2000)
                .NotNull();
        }
    }
}
