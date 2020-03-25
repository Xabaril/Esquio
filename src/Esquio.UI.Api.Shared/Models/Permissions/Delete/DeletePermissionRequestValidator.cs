using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Permissions.Delete
{
    public class DeletePermissionRequestValidator
         : AbstractValidator<DeletePermissionRequest>
    {
        public DeletePermissionRequestValidator()
        {
            this.RuleFor(rf => rf.SubjectId)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
