using Hackathon.Application.Common.Models;
using Hackathon.Application.DTOs;
using Hackathon.Application.Users.Commands.CreateUser;
using Hackathon.Application.Users.Queries.GetUserByIdQuery;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> CreateUser(CreateUserCommand command)
    {
        var result = await Mediator.Send(command);

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] int id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Mediator.Send(query);

        return result.Match<UserDto, ActionResult>(
            Ok,
            errors => NotFound(string.Join(", ", errors))
        );
    }
}