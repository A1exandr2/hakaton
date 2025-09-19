using Hackathon.Application.DTOs.Exceptions;
using Hellang.Middleware.ProblemDetails;

namespace CleanArchitecture.API.Common;

public static class GlobalExceptionHandler
{
    public static IServiceCollection AddGlobalExceptionHandler(
        this IServiceCollection services
    )
    {
        services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (ctx, ex) => false;

            options.Map<ValidationException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
            });

            options.Map<NotFoundException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message,
            });

            options.Map<Hackathon.Domain.Exceptions.DomainException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Domain Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
            });
        });

        return services;
    }
}