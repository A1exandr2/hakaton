using Hackathon.Domain.Entities;

namespace Hackathon.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
}