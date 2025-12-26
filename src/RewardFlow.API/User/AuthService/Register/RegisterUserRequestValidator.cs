using FluentValidation;
using static Reward_Flow_v2.User.AuthService.Register.Register;

namespace Reward_Flow_v2.User.AuthService.Register;

public class RegisterUserRequestValidator : AbstractValidator<Request>
{
    public RegisterUserRequestValidator()
    {
        // Username
        RuleFor(x => x.username).NotEmpty();
        RuleFor(x => x.username).NotNull();
        RuleFor(x => x.username).MinimumLength(3);

        // Password
        RuleFor(x => x.password).NotEmpty();
        RuleFor(x => x.password).NotNull();
        RuleFor(x => x.password).MinimumLength(8);

        // Email
        RuleFor(x => x.email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.email));
    }

}


