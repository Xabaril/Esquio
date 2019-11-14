using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.ApiKeys.Add
{
    public class AddApiKeyRequestValidator
        : AbstractValidator<AddApiKeyRequest>
    {
        public AddApiKeyRequestValidator()
        {
            this.RuleFor(f => f.Name)
                .MaximumLength(200)
                .NotNull();
        }
    }
}
