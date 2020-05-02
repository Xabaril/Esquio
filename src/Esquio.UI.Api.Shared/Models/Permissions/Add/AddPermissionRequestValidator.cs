using FluentValidation;
using System;

namespace Esquio.UI.Api.Shared.Models.Permissions.Add
{
    public class AddPermissionRequestValidator
        : AbstractValidator<AddPermissionRequest>
    {
        public AddPermissionRequestValidator()
        {
            this.RuleFor(ar => ar.SubjectId)
                .NotNull()
                .NotEmpty();

            this.RuleFor(ar => ar.ActAs)
                .Must(value =>
                value.Equals("Reader", StringComparison.InvariantCultureIgnoreCase) 
                ||
                value.Equals("Contributor", StringComparison.InvariantCultureIgnoreCase)
                ||
                value.Equals("Management", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
