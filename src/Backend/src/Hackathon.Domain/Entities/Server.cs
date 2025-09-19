using System.Net.Http.Headers;

namespace Hackathon.Domain.Entities;

public class Server : BaseEntity
{
    public required string Name { get; set; }
    public required string Ip { get; set; }

    public Server(string name, string ip, int id) : base(id)
    {
        Name = string.IsNullOrWhiteSpace(name) ?
        throw new ArgumentException("IP cannot be null or empty", nameof(ip)) : name;

        Ip = string.IsNullOrWhiteSpace(ip) ?
        throw new ArgumentException("IP cannot be null or empty", nameof(ip)) : ip;
    }
}