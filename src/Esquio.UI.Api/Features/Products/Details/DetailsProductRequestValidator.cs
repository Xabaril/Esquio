using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Products.Details
{
    public class DetailsProductRequestValidator
        : AbstractValidator<DetailsProductRequest>
    {
        public DetailsProductRequestValidator()
        {
            this.RuleFor(rf => rf.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200)
                .Matches("^[a-zA-Z][a-zA-Z0-9]*$");
        }
    }
}
