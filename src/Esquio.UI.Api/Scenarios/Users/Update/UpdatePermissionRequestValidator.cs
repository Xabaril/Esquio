using Esquio.UI.Api.Infrastructure.Data.Entities;
using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Users.Update
{

    public class UpdatePermissionRequestValidator
        : AbstractValidator<UpdatePermissionRequest>
    {
        public UpdatePermissionRequestValidator()
        {
            this.RuleFor(p => p.ActAs)
                .IsEnumName(typeof(ApplicationRole), false);

            this.RuleFor(p => p.SubjectId)
                .NotEmpty();
        }
    }
}
