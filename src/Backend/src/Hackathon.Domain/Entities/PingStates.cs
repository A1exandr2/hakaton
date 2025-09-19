namespace Hackathon.Domain.Entities;

public class PingStats
{
    public long TotalPings { get; set; }
    public long SuccessfulPings { get; set; }
    public double AvgResponseTimeMs { get; set; }
    public DateTime? LastCheck { get; set; }

    public PingStats(long totalPings, long successfulPings, double avgResponseTimeMs, DateTime? lastCheck)
    {
        TotalPings = totalPings;
        SuccessfulPings = successfulPings;
        AvgResponseTimeMs = avgResponseTimeMs;
        LastCheck = lastCheck;
    }
}