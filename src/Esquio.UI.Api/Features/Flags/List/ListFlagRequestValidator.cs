using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Flags.List
{
    public class ListFlagRequestValidator
        : AbstractValidator<ListFlagRequest>
    {
        public ListFlagRequestValidator()
        {
            this.RuleFor(rf => rf.PageIndex)
                .GreaterThanOrEqualTo(0)
                .LessThan(Int32.MaxValue);

            this.RuleFor(rf => rf.PageCount)
                .GreaterThan(0)
                .LessThan(100);
        }
    }
}
