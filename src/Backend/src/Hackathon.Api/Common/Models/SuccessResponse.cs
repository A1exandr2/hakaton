using Hackathon.Domain.Entities;

namespace Hackathon.Api.Models;

public class SuccessResponse<T> : ApiResponse
{
    public required T Data { get; init; }
}