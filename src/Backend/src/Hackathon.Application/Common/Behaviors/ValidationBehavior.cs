using FluentValidation;
using MediatR;

namespace Hackathon.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    ) 
    {
        if (_validators.Any())
        {
            var failtures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(f => f.Errors)
            .Where(f => f is not null)
            .ToList();

            if (failtures.Count != 0)
            {
                throw new ValidationException(failtures);
            }
        }

        return await next();
    }
}