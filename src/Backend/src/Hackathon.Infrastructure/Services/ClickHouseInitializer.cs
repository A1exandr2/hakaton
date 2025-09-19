// Hackathon.Infrastructure/Services/ClickHouseInitializer.cs

using ClickHouse.Client.ADO;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hackathon.Infrastructure.Services;

public class ClickHouseInitializer
{
    private readonly IConfiguration _configuration;

    public ClickHouseInitializer(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString("ClickHouse");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("ClickHouse connection string is not configured.");

        await using var connection = new ClickHouseConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await ExecuteCommandAsync(connection, "CREATE DATABASE IF NOT EXISTS monitoring", cancellationToken);
        await ExecuteCommandAsync(connection, "USE monitoring", cancellationToken);

        bool tableExists = await TableExistsAsync(connection, "ping_logs", cancellationToken);
        if (!tableExists)
        {
            await CreatePingLogsTableAsync(connection, cancellationToken);
            return;
        }
    }

    private async Task<bool> TableExistsAsync(ClickHouseConnection connection, string tableName, CancellationToken ct)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT count() 
            FROM system.tables 
            WHERE database = 'monitoring' AND name = @tableName";

        var param = cmd.CreateParameter();
        param.ParameterName = "tableName";
        param.Value = tableName;
        cmd.Parameters.Add(param);

        var result = await cmd.ExecuteScalarAsync(ct);
        return Convert.ToUInt64(result) > 0;
    }

    private async Task CreatePingLogsTableAsync(ClickHouseConnection connection, CancellationToken ct)
    {
        const string createTableQuery = @"
            CREATE TABLE monitoring.ping_logs (
                id Int32,
                server_id UInt32,
                timestamp DateTime64(3) DEFAULT now64(3),
                response_time_ms Float32,
                success UInt8 DEFAULT 1,
                error_message String DEFAULT ''
            ) ENGINE = MergeTree()
            ORDER BY (server_id, timestamp)
            TTL timestamp + INTERVAL 90 DAY;
        ";

        await ExecuteCommandAsync(connection, createTableQuery, ct);
    }

    private static async Task ExecuteCommandAsync(ClickHouseConnection connection, string commandText, CancellationToken ct)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = commandText;
        await cmd.ExecuteNonQueryAsync(ct);
    }
}