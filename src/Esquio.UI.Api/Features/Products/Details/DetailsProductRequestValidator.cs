using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Products.Details
{
    public class DetailsProductRequestValidator
        : AbstractValidator<DetailsProductRequest>
    {
        public DetailsProductRequestValidator()
        {
            this.RuleFor(rf => rf.ProductId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);
        }
    }
}
