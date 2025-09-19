using Hackathon.Domain.Entities;

namespace Hackathon.Api.Models;

public class SuccessResponse<T> where T : BaseEntity
{
    public required T Data { get; init; }
}