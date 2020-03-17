using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Features.Add
{
    public class AddFeatureRequestValidator
        : AbstractValidator<AddFeatureRequest>
    {
        public AddFeatureRequestValidator()
        {
            this.RuleFor(f => f.Name)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(Constants.Constraints.NamesRegularExpression);

            this.RuleFor(f => f.Description)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(2000);
        }
    }
}
