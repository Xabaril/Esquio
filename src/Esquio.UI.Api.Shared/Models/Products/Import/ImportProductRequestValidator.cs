using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Products.Import
{
    public class ImportProductRequestValidator
        : AbstractValidator<ImportProductRequest>
    {
        public ImportProductRequestValidator()
        {
            this.RuleFor(rf => rf.Content)
                .NotEmpty();
        }
    }
}
