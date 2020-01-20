using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Products.AddRing
{
    public class AddRingRequestValidator
       : AbstractValidator<AddRingRequest>
    {
        public AddRingRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(ApiConstants.Constraints.NamesRegularExpression);
        }
    }
}
