using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.Add
{
    public class AddFeatureRequestValidator
        : AbstractValidator<AddFeatureRequest>
    {
        public AddFeatureRequestValidator()
        {
            this.RuleFor(f => f.Name)
                .MaximumLength(200)
                .NotNull();

            this.RuleFor(f => f.Description)
                .MaximumLength(2000)
                .NotNull();
        }
    }
}
