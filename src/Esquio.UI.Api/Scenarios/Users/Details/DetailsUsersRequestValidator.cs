using FluentValidation;

namespace Esquio.UI.Api.Scenarios.Users.Details
{
    public class DetailsUsersRequestValidator
         : AbstractValidator<DetailsUsersRequest>
    {
        public DetailsUsersRequestValidator()
        {
            this.RuleFor(rf => rf.SubjectId)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
