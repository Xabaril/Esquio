using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Products.List
{
    public class ListProductRequestValidator
        : AbstractValidator<ListProductRequest>
    {
        public ListProductRequestValidator()
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
