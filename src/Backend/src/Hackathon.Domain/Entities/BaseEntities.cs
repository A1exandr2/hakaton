using System.Runtime.CompilerServices;

namespace Hackathon.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; protected set; }

    public BaseEntity(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be greater than zero.", nameof(id));
        Id = id;
    }
}