namespace Hackathon.Domain.Entities;

public class PingLog
{
    public uint ServerId { get; set; } 
    public DateTime Timestamp { get; set; }
    public float ResponseTimeMs { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = null!;

    public PingLog(DateTime timestamp,
        uint serverId,
        float responseTimeMs,
        bool success,
        string errorMessage)
    {
        ServerId = serverId;
        Timestamp = timestamp;
        ResponseTimeMs = responseTimeMs;
        Success = success;
        ErrorMessage = errorMessage;
    }
}