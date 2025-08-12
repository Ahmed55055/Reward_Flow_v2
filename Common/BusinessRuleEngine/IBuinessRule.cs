namespace Reward_Flow_v2.Common.BusinessRuleEngine
{
    public interface IBusinessRule
    {
        bool IsMet();
        string Error { get; }
    }
}
