using FluentValidation;
using System;

namespace Esquio.UI.Api.Shared.Models.Permissions.Update
{
    public class UpdatePermissionRequestValidator
        : AbstractValidator<UpdatePermissionRequest>
    {
        public UpdatePermissionRequestValidator()
        {
            this.RuleFor(ar => ar.ActAs)
               .Must(value =>
               value.Equals("Reader", StringComparison.InvariantCultureIgnoreCase)
               ||
               value.Equals("Contributor", StringComparison.InvariantCultureIgnoreCase)
               ||
               value.Equals("Management", StringComparison.InvariantCultureIgnoreCase));

            this.RuleFor(p => p.SubjectId)
                .NotEmpty();
        }
    }
}
