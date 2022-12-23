using MicroservicesCrudManager.Core;
using Microsoft.AspNetCore.Mvc;
using WebApi.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<StorageManager>();
builder.Services.AddScoped<TestAdd>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/v1/{entity}",
    (HttpContext httpContext, StorageManager storageManager, string entity) =>
    {
        try
        {
            var payload = httpContext.Request.Body.Deserialize(entity);

            if (payload == null)
            {
                throw new ArgumentNullException($"No payload provided in /api/v1/{entity} POST");
            }

            var result = storageManager.ActivateAdd(entity, payload);

            return Results.Created($"/api/v1/{entity}", result);
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException)
            {
                return Results.BadRequest(ex.Message);
            }

            return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    });

app.MapPut("/api/v1/{entity}",
    (HttpContext httpContext, StorageManager storageManager, string entity) =>
    {
        try
        {
            var payload = httpContext.Request.Body.Deserialize(entity);

            var result = storageManager.ActivateUpdate(entity, payload);

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException)
            {
                return Results.BadRequest(ex.Message);
            }

            return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    });

app.Run();