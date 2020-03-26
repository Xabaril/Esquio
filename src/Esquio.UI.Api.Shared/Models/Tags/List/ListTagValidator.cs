using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Tags.List
{
    public class ListTagValidator 
        : AbstractValidator<ListTagRequest>
    {
        public ListTagValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);

            RuleFor(x => x.FeatureName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);
        }
    }
}
