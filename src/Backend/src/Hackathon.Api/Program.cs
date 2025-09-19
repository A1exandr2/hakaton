using Hackathon.Infrastructure;
using Hackathon.Infrastructure.Services;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

await app.AddClickHouse();

app.UseHttpsRedirection();

app.Run();
