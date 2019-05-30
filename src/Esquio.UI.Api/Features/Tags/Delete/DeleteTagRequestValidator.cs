using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Tags.Delete
{
    public class DeleteTagRequestValidator
        : AbstractValidator<DeleteTagRequest>
    {
        public DeleteTagRequestValidator()
        {
            this.RuleFor(rf => rf.Tag)
                .NotNull();

            this.RuleFor(rf => rf.FeatureId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);
        }
    }
}
