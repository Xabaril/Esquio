using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.Add
{
    public class AddFlagRequestValidator
        : AbstractValidator<AddFlagRequest>
    {
        public AddFlagRequestValidator()
        {
            this.RuleFor(f=>f.ProductId)
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
