using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Flags.Add
{
    public class AddFeatureRequestValidator
        : AbstractValidator<AddFeatureRequest>
    {
        public AddFeatureRequestValidator()
        {
            this.RuleFor(f => f.Name)
                .MaximumLength(200)
                .NotNull()
                .Matches(ApiConstants.Constraints.NamesRegularExpression);

            this.RuleFor(f => f.Description)
                .MaximumLength(2000)
                .NotNull();
        }
    }
}
