using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.Details
{
    class DetailsFlagRequestValidator
        : AbstractValidator<DetailsFlagRequest>
    {
        public DetailsFlagRequestValidator()
        {
            this.RuleFor(rf => rf.ProductId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);

            this.RuleFor(rf => rf.FeatureId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);
        }
    }
}
