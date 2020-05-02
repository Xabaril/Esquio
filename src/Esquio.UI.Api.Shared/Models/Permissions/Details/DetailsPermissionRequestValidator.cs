using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Permissions.Details
{
    public class DetailsPermissionRequestValidator
         : AbstractValidator<DetailsPermissionRequest>
    {
        public DetailsPermissionRequestValidator()
        {
            this.RuleFor(rf => rf.SubjectId)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
