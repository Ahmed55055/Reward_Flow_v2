
using FluentResults;

namespace Reward_Flow_v2.Common.FluantResult;

public class StatusCodesErrors : Error
{
    public int StatusCode;

    public StatusCodesErrors(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
    public StatusCodesErrors(int statusCode) : base()
    {
        StatusCode = statusCode;
    }
}
