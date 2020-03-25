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

            //TODO: add validation here
            //RuleFor(x => x.ActAs)
            //    .IsEnumName(typeof(ApplicationRole), caseSensitive: false);
        }
    }
}
