using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Tags.List
{
    public class ListTagValidator 
        : AbstractValidator<ListTagRequest>
    {
        public ListTagValidator()
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
        }
    }
}
