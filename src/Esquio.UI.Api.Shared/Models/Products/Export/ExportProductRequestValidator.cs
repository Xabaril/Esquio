using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Products.Export
{
    public class ExportProductRequestValidator
        : AbstractValidator<ExportProductRequest>
    {
        public ExportProductRequestValidator()
        {
            this.RuleFor(rf => rf.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);
        }
    }
}
