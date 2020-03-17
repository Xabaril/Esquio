using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Products.AddRing
{
    public class AddRingRequestValidator
       : AbstractValidator<AddRingRequest>
    {
        public AddRingRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(Constants.Constraints.NamesRegularExpression);
        }
    }
}
