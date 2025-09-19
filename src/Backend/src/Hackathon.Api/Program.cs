using CleanArchitecture.API.Common;
using Hackathon.Application;
using Hackathon.Infrastructure;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddGlobalExceptionHandler();

var app = builder.Build();

app.UseProblemDetails();

await app.AddClickHouse();

app.UseHttpsRedirection();

app.Run();
