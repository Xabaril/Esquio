using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteFlagRequestValidator
        : AbstractValidator<DeleteFlagRequest>
    {
        public DeleteFlagRequestValidator()
        {
            this.RuleFor(rf => rf.FeatureName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(ApiConstants.Constraints.NamesRegularExpression);

            this.RuleFor(rf => rf.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches(ApiConstants.Constraints.NamesRegularExpression);
        }
    }
}
