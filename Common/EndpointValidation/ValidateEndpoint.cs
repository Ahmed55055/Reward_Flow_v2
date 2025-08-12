using FluentValidation;

namespace Reward_Flow_v2.Common.EndpointValidation;

public static class ValidateEndpoint
{
    public static IEndpointConventionBuilder Validation<T>(this IEndpointConventionBuilder builder, AbstractValidator<T> validator)
    {
        builder.AddEndpointFilter(async (context, next) =>
        {
            var request = context.Arguments.OfType<T>().FirstOrDefault();

            if (request is null)
                return Results.BadRequest();

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            var result = await next(context);

            return result;
        });

        return builder;
    }
}
