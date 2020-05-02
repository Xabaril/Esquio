using FluentValidation;
using System;

namespace Esquio.UI.Api.Shared.Models.ApiKeys.Add
{
    public class AddApiKeyRequestValidator
        : AbstractValidator<AddApiKeyRequest>
    {
        public AddApiKeyRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(Constants.Constraints.NamesRegularExpression);

            RuleFor(x => x.ValidTo)
                .GreaterThan(DateTime.UtcNow)
                .When(a => a.ValidTo != null);

            this.RuleFor(ar => ar.ActAs)
               .Must(value =>
               value.Equals("Reader", StringComparison.InvariantCultureIgnoreCase)
               ||
               value.Equals("Contributor", StringComparison.InvariantCultureIgnoreCase)
               ||
               value.Equals("Management", StringComparison.InvariantCultureIgnoreCase));

        }
    }
}
