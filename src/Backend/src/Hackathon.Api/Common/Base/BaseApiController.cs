using Hackathon.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult<T> HandleResult<T>(Result<T> result)
    {
        if (result.Succeeded) return Ok(result.Value);
        return BadRequest(result.Errors);
    }
}