using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Tags.Add
{
    public class AddTagValidator 
        : AbstractValidator<AddTagRequest>
    {
        public AddTagValidator()
        {
            this.RuleFor(rf => rf.Tag)
                .NotNull();

            this.RuleFor(rf => rf.FeatureId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);
        }
    }
}
