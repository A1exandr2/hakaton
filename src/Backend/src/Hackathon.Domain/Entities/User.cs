using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Hackathon.Domain.Exceptions;

namespace Hackathon.Domain.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public bool IsDevOps { get; set; }
    public string? Tg { get; set; }

    public User(string email, bool isDevOps, int id, string? tg) : base(id)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("email is null");
        if (string.IsNullOrWhiteSpace(tg))
            throw new DomainException("email is null");
        Email = email;
        Tg = tg;
        IsDevOps = isDevOps;
    }
}