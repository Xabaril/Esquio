using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Users.Update
{
    public class UpdatePermissionRequestValidator
        : AbstractValidator<UpdatePermissionRequest>
    {
        public UpdatePermissionRequestValidator()
        {
            //TODO: fix it
            //this.RuleFor(p => p.ActAs)
            //    .IsEnumName(typeof(ApplicationRole), false);

            this.RuleFor(p => p.SubjectId)
                .NotEmpty();
        }
    }
}
