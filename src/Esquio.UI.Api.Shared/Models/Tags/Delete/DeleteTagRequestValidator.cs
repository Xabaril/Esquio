using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Tags.Delete
{
    public class DeleteTagRequestValidator
        : AbstractValidator<DeleteTagRequest>
    {
        public DeleteTagRequestValidator()
        {
            this.RuleFor(rf => rf.Tag)
                .NotNull();

            RuleFor(x => x.FeatureName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);
        }
    }
}
