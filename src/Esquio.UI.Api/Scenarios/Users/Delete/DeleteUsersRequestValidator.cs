using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Users.Delete
{
    public class DeleteUsersRequestValidator
         : AbstractValidator<DeleteUsersRequest>
    {
        public DeleteUsersRequestValidator()
        {
            this.RuleFor(rf => rf.SubjectId)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
