using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Features.Add
{
    public class AddFeatureRequestValidator
        : AbstractValidator<AddFeatureRequest>
    {
        public AddFeatureRequestValidator()
        {
            this.RuleFor(f => f.Name)
                .MaximumLength(200)
                .NotNull()
                .Matches(Constants.Constraints.NamesRegularExpression);

            this.RuleFor(f => f.Description)
                .MaximumLength(2000)
                .NotNull();
        }
    }
}
