namespace Hackathon.Application.DTOs;

public class ServerInfo
{
    public int Id { get; set; }
    public string? Ip { get; set; }
    public ServerStatus serverStatus { get; set; }
    public List<LogDto>? Logs { get; set; }
    public StatsDto? Stats { get; set; }
}