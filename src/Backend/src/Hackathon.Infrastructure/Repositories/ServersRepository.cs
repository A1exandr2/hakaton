using Hackathon.Domain.Repositories;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Hackathon.Application.DTOs.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class ServersRepository : IServersRepository
{
    private readonly AppDbContext _context;

    public ServersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Server> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _context.Servers.FindAsync(id, cancellationToken) ?? throw new NotFoundException(nameof(User), id);

    public async Task<List<Server>> GetAllAsync(int page, int paggination, string query, CancellationToken cancellationToken)
    {
        var serversQuery = _context.Servers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.ToLower();

            serversQuery = serversQuery.Where(w => w.Name.ToLower().Contains(normalizedQuery));
        }

        return await serversQuery.OrderBy(w => w.Id).Skip((page - 1) * paggination).Take(paggination).ToListAsync(cancellationToken);
    }
    public async Task<int> GetPageCountAsync(int pagination, string? query, CancellationToken cancellationToken)
    {
        var serversQuery = _context.Servers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.ToLower();

            serversQuery = serversQuery.Where(w => w.Name.ToLower().Contains(normalizedQuery));
        }

        return (int)Math.Ceiling(await serversQuery.CountAsync(cancellationToken) / (double)pagination);
    }
}