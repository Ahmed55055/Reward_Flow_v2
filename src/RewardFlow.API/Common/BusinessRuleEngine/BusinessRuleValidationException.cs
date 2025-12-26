namespace Reward_Flow_v2.Common.BusinessRuleEngine;

public class BusinessRuleValidationException : InvalidOperationException
{
    internal BusinessRuleValidationException(string message) : base(message)
    {
    }
}