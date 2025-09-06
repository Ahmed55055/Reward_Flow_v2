using FluentValidation;

namespace Reward_Flow_v2.Rewards.SessionsReward.UpdateSessionsReward;
internal class UpdateSessionsRewardValidator : AbstractValidator<UpdateSessionsRewardRequest>
{
    public UpdateSessionsRewardValidator()
    {
        RuleFor(x => x.RewardName.Value)
            .NotEmpty()
            .When(x => x.RewardName.HasValue);

        RuleFor(x => x.Percentage.Value)
            .GreaterThan(0)
            .When(x => x.Percentage.HasValue);

    }
}