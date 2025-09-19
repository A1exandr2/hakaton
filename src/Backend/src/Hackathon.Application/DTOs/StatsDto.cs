namespace Hackathon.Application.DTOs;

public class StatsDto
{
    public int TotalPings { get; set; }
    public double SuccessRate { get; set; }
    public double AvgResponseTimeMs { get; set; }
    public DateTime LastCheck { get; set; }
}