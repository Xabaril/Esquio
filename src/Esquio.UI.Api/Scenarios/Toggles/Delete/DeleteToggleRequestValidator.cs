using FluentValidation;

namespace Esquio.UI.Api.Features.Toggles.Delete
{
    public class DeleteToggleRequestValidator
        : AbstractValidator<DeleteToggleRequest>
    {
        public DeleteToggleRequestValidator()
        {
            RuleFor(x => x.ProductName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200)
               .Matches(ApiConstants.Constraints.NamesRegularExpression);

            RuleFor(x => x.FeatureName)
               .NotEmpty()
               .MinimumLength(5)
               .MaximumLength(200)
               .Matches(ApiConstants.Constraints.NamesRegularExpression);

            this.RuleFor(pr => pr.ToggleType)
                .NotEmpty();

        }
    }
}
