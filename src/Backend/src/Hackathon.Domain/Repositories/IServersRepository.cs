using Hackathon.Domain.Entities;

namespace Hackathon.Domain.Repositories;

public interface IServersRepository
{
    Task<Server> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<Server>> GetAllAsync(int page, int paggination, string query, CancellationToken cancellationToken);
    Task<int> GetPageCountAsync(int pagination, string? query, CancellationToken cancellationToken);

}