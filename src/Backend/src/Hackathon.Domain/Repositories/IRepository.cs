using Hackathon.Domain.Entities;

public interface IRepository<TEntity, TKey> where TEntity : BaseEntity
{
    Task<TEntity> GetByIdAsync(TKey id, CancellationToken ct);
    Task<List<TEntity>> GetAllAsync(int page, int paggination, string query, CancellationToken ct);
    Task<int> GetPageCountAsync(int pagination, string? query, CancellationToken ct);
}