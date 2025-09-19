namespace Hackathon.Application.DTOs;

public class LogDto
{
    public DateTime Timestamp { get; set; }
    public double ResponseTimeMs { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}