// Hackathon.Infrastructure/Repositories/PingLogsRepository.cs
using ClickHouse.Client.ADO;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text;
using Hackathon.Application.DTOs.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Repositories;

namespace Hackathon.Infrastructure.Repositories;

public class PingLogsRepository : IPingLogsRepository
{
    private readonly string _connectionString;

    public PingLogsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ClickHouse") 
            ?? throw new InvalidOperationException("ClickHouse connection string is not configured.");
    }

    public async Task InsertAsync(PingLog log, CancellationToken ct = default)
    {
        const string query = @"
            INSERT INTO monitoring.ping_logs 
            (id, server_id, timestamp, response_time_ms, success, error_message)
            VALUES (@id, @server_id, @timestamp, @response_time_ms, @success, @error_message)";

        await using var connection = new ClickHouseConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        AddParameter(cmd, "id", log.Id);
        AddParameter(cmd, "server_id", log.ServerId);
        AddParameter(cmd, "timestamp", log.Timestamp);
        AddParameter(cmd, "response_time_ms", log.ResponseTimeMs);
        AddParameter(cmd, "success", log.Success ? (byte)1 : (byte)0);
        AddParameter(cmd, "error_message", log.ErrorMessage);

        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task InsertBatchAsync(IEnumerable<PingLog> logs, CancellationToken ct = default)
    {
        var sb = new StringBuilder();
        sb.AppendLine("INSERT INTO monitoring.ping_logs (id, server_id, timestamp, response_time_ms, success, error_message) VALUES");

        var isFirst = true;
        foreach (var log in logs)
        {
            if (!isFirst) sb.Append(",");
            isFirst = false;

            sb.Append($"(");
            sb.Append($"{log.Id}, "); // Int32 → не нужно оборачивать в ''
            sb.Append($"{log.ServerId}, ");
            sb.Append($"'{log.Timestamp:yyyy-MM-dd HH:mm:ss.fff}', ");
            sb.Append($"{log.ResponseTimeMs.ToString(CultureInfo.InvariantCulture)}, ");
            sb.Append($"{(log.Success ? 1 : 0)}, ");
            sb.Append($"'{EscapeString(log.ErrorMessage)}'");
            sb.Append($")");
        }

        await using var connection = new ClickHouseConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = sb.ToString();
        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task<List<PingLog>> GetByServerIdAsync(uint serverId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        const string query = @"
            SELECT id, timestamp, response_time_ms, success, error_message
            FROM monitoring.ping_logs
            WHERE server_id = @server_id AND timestamp >= @from AND timestamp <= @to
            ORDER BY timestamp DESC";

        await using var connection = new ClickHouseConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        AddParameter(cmd, "server_id", serverId);
        AddParameter(cmd, "from", from);
        AddParameter(cmd, "to", to);

        var logs = new List<PingLog>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            logs.Add(new PingLog(
                reader.GetDateTime(1),
                serverId,
                reader.GetFloat(2),
                reader.GetFieldValue<byte>(3) == 1,
                reader.GetString(4),
                reader.GetFieldValue<int>(0)
            ));
        }

        return logs;
    }

    public async Task<PingLog> GetLastByServerIdAsync(uint serverId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        const string query = @"
            SELECT id, timestamp, response_time_ms, success, error_message
            FROM monitoring.ping_logs
            WHERE server_id = @server_id AND timestamp >= @from AND timestamp <= @to
            ORDER BY timestamp DESC
            LIMIT 1";

        await using var connection = new ClickHouseConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        AddParameter(cmd, "server_id", serverId);
        AddParameter(cmd, "from", from);
        AddParameter(cmd, "to", to);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new PingLog(
                reader.GetDateTime(1),
                serverId,
                reader.GetFloat(2),
                reader.GetFieldValue<byte>(3) == 1,
                reader.GetString(4),
                reader.GetFieldValue<int>(0)
            );
        }

        throw new NotFoundException(nameof(PingLog), serverId);
    }

    public async Task<PingStats> GetStatsAsync(uint serverId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        const string query = @"
            SELECT 
                count() as total_pings,
                sum(success) as successful_pings,
                if(sum(success) > 0, avgIf(response_time_ms, success = 1), 0) as avg_response_time_ms,
                max(timestamp) as last_check
            FROM monitoring.ping_logs
            WHERE server_id = @server_id AND timestamp >= @from AND timestamp <= @to";

        await using var connection = new ClickHouseConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        AddParameter(cmd, "server_id", serverId);
        AddParameter(cmd, "from", from);
        AddParameter(cmd, "to", to);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new PingStats(
                reader.GetFieldValue<long>(0),
                reader.GetFieldValue<long>(1),
                reader.GetFieldValue<double>(2),
                reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3)
            );
        }

        return new PingStats(0, 0, 0, null);
    }

    private static void AddParameter(ClickHouseCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }

    private static string EscapeString(string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        return s.Replace("'", "''").Replace("\\", "\\\\");
    }
}