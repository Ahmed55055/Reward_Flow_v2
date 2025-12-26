namespace Reward_Flow_v2.Common.BusinessRuleEngine
{
    public class BusinessRuleValidator 
    {
        internal static void Validate(IBusinessRule rule)
        {
            if(!rule.IsMet())
                throw new BusinessRuleValidationException(rule.Error);
        }
    }
}
