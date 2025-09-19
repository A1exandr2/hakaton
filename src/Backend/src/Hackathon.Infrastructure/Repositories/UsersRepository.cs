using Hackathon.Domain.Repositories;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Hackathon.Application.DTOs.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class UsersRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UsersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _context.Users.FindAsync(id, cancellationToken) ?? throw new NotFoundException(nameof(User), id);

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.Users.ToListAsync(cancellationToken) ?? throw new NotFoundException();
}