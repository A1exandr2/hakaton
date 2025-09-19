using Microsoft.Extensions.Logging;
using MediatR;

namespace Hackathon.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {

        var name = typeof(TRequest).Name;
        
        try
        {
            _logger.LogDebug(
                $"[MediatR] начало запроса {name} в {DateTime.UtcNow}"
            );

            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"[MediatR] ошибка в {name} в {DateTime.UtcNow}"
            );

            throw;
        }
        }
}