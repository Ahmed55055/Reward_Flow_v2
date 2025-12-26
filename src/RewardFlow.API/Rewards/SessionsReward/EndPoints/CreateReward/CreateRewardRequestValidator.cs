using FluentValidation;

namespace Reward_Flow_v2.Rewards.SessionsReward.CreateReward;

public static partial class CreateSessionsReward
{
    public class CreateRewardRequestValidator : AbstractValidator<Request>
    {
        public CreateRewardRequestValidator()
        {

            RuleFor(x => x.RewardName)
                .NotEmpty()
                .When(x => x.RewardName is not null);

            RuleFor(x => x.RewardCode)
                .NotEmpty()
                .MaximumLength(50)
                .When(x => x.RewardCode is not null);

            RuleFor(x => x.Year)
                .GreaterThan(2011)
                .When(x => x.Year is not null);

            RuleFor(x => x.Semester)
                .GreaterThan((byte)0)
                .When(x => x.Semester is not null);

            RuleFor(x => x.Percentage)
                .GreaterThan(0);
        }
    }
}