using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Tags.Add
{
    public class AddTagValidator 
        : AbstractValidator<AddTagRequest>
    {
        public AddTagValidator()
        {
            this.RuleFor(rf => rf.Tag)
                .NotNull()
                .Matches(Constants.Constraints.NamesRegularExpression);

            this.RuleFor(rf => rf.HexColor)
                .MaximumLength(7)
                .Matches(Constants.Constraints.HexColorRegularExpression)
                .When(rf => rf.HexColor is object);
        }
    }
}
