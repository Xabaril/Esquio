using Esquio.UI.Api.Shared.Infrastructure.Data.Entities;
using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Users.Add
{
    public class AddPermissionRequestValidator
        : AbstractValidator<AddPermissionRequest>
    {
        public AddPermissionRequestValidator()
        {
            this.RuleFor(p => p.ActAs)
                .IsEnumName(typeof(ApplicationRole), caseSensitive: false);
        }
    }
}
