using FluentValidation;

namespace Esquio.UI.Api.Shared.Models.Users.Add
{
    public class AddPermissionRequestValidator
        : AbstractValidator<AddPermissionRequest>
    {
        public AddPermissionRequestValidator()
        {
            this.RuleFor(ar => ar.SubjectId)
                .NotNull()
                .NotEmpty();

            //TODO
            //this.RuleFor(p => p.ActAs)
            //    .IsEnumName(typeof(ApplicationRole), caseSensitive: false);
        }
    }
}
