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
        }
    }
}
