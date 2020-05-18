using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Products.AddDeployment
{
    public class AddDeploymentRequestValidator
       : AbstractValidator<AddDeploymentRequest>
    {
        public AddDeploymentRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(Constants.Constraints.NamesRegularExpression);
        }
    }
}
