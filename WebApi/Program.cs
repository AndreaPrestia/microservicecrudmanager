using MicroservicesCrudManager.Core;
using WebApi.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.AddMscm();
builder.Services.AddScoped<TestAdd>();

var app = builder.Build();

app.MapMscmEndpoints();

app.Run();