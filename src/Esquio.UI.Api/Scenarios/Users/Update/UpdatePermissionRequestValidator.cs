using FluentValidation;

namespace Esquio.UI.Api.Features.Users.Update
{

    public class UpdatePermissionRequestValidator
        : AbstractValidator<UpdatePermissionRequest>
    {
        public UpdatePermissionRequestValidator()
        {
            this.RuleFor(p => p.Read)
                .Equal(true)
                .When(p => p.Write);

            this.RuleFor(p => p.Write)
                .Equal(true)
                .When(p => p.Manage);
        }
    }
}
