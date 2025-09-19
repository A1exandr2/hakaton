using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.Domain.Entities;

namespace Hackathon.Domain.Repositories;

public interface IPingLogsRepository
{
    Task InsertAsync(PingLog log, CancellationToken ct = default);
    Task InsertBatchAsync(IEnumerable<PingLog> logs, CancellationToken ct = default);
    Task<List<PingLog>> GetByServerIdAsync(uint serverId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<PingLog?> GetLastByServerIdAsync(uint serverId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<PingStats> GetStatsAsync(uint serverId, DateTime from, DateTime to, CancellationToken ct = default);
}