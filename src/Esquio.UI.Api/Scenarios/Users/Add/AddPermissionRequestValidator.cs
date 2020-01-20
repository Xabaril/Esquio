using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Users.Add
{
    public class AddPermissionRequestValidator
        : AbstractValidator<AddPermissionRequest>
    {
        public AddPermissionRequestValidator()
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
