using FluentValidation;
using System;

namespace Esquio.UI.Api.Features.Tags.List
{
    public class ListTagValidator 
        : AbstractValidator<ListTagRequest>
    {
        public ListTagValidator()
        {
            this.RuleFor(rf => rf.FeatureId)
                .GreaterThan(0)
                .LessThan(Int32.MaxValue);
        }
    }
}
