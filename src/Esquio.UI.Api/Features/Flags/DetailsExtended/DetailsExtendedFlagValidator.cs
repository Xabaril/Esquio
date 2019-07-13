using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.DetailsExtended
{
    public class DetailsExtendedFlagValidator
      : AbstractValidator<DetailsExtendedFlagRequest>
    {
        public DetailsExtendedFlagValidator()
        {
            this.RuleFor(rf => rf.FeatureId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);
        }
    }
}
