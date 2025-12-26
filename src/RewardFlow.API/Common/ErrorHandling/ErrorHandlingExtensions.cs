namespace Reward_Flow_v2.Common.ErrorHandling
{
    public static class ErrorHandlingExtensions
    {
        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }
    }
}
