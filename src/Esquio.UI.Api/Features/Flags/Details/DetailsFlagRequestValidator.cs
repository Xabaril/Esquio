using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequestValidator
        : AbstractValidator<DetailsFlagRequest>
    {
        public DetailsFlagRequestValidator()
        {
            this.RuleFor(rf => rf.FeatureId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);
        }
    }
}
