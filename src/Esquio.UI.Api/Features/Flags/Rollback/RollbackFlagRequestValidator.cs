using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.Rollback
{
    class RollbackFlagRequestValidator
        : AbstractValidator<RollbackFlagRequest>
    {
        public RollbackFlagRequestValidator()
        {

            this.RuleFor(rf => rf.FeatureId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);
        }
    }
}
