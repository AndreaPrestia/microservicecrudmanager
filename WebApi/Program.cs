using MicroServicesCrudManager.Core;
using WebApi.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.AddMcm();
builder.Services.AddScoped<TestAdd>();

var app = builder.Build();

app.MapMcmEndpoints();

app.Run();