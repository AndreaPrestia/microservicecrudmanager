using MicroservicesCrudManager.Core;
using WebApi.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<StorageManager>();
builder.Services.AddScoped<TestAdd>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/v1/{entity}",
    (HttpContext httpContext,  StorageManager storageManager, string entity) =>
    {
        try
        {
            var result = storageManager.ActivateAdd(entity, httpContext.Request.Body.Deserialize(entity));

            return Results.Created($"/api/v1/{entity}", result);
        }
        catch (Exception ex)
        {
            return ex switch
            {
                ArgumentException => Results.BadRequest(ex.Message),
                EntryPointNotFoundException => Results.NotFound(ex.Message),
                _ => Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError)
            };
        }
    });

app.MapPut("/api/v1/{entity}/{id}",
    (HttpContext httpContext, StorageManager storageManager, string entity, string id) =>
    {
        try
        {
            var result = storageManager.ActivateUpdate(entity, id, httpContext.Request.Body.Deserialize(entity));

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return ex switch
            {
                ArgumentException => Results.BadRequest(ex.Message),
                EntryPointNotFoundException => Results.NotFound(ex.Message),
                _ => Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError)
            };
        }
    });

app.MapDelete("/api/v1/{entity}/{id}",
    (HttpContext httpContext, StorageManager storageManager, string entity, string id) =>
    {
        try
        {
            var result = storageManager.ActivateDelete(entity, id);

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return ex switch
            {
                ArgumentException => Results.BadRequest(ex.Message),
                EntryPointNotFoundException => Results.NotFound(ex.Message),
                _ => Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError)
            };
        }
    });

app.MapGet("/api/v1/{entity}/{id}",
    (HttpContext httpContext, StorageManager storageManager, string entity, string id) =>
    {
        try
        {
            var result = storageManager.ActivateGet(entity, id);

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return ex switch
            {
                ArgumentException => Results.BadRequest(ex.Message),
                EntryPointNotFoundException => Results.NotFound(ex.Message),
                _ => Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError)
            };
        }
    });


app.MapGet("/api/v1/{entity}/{page:int}/{limit:int}/{query}/{orderBy}/{ascending:bool}",
    (HttpContext httpContext, StorageManager storageManager, string entity, int page, int limit, string query,
        string orderBy, bool ascending) =>
    {
        try
        {
            var result = storageManager.ActivateList(entity, page, limit, query, orderBy, ascending);

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return ex switch
            {
                ArgumentException => Results.BadRequest(ex.Message),
                EntryPointNotFoundException => Results.NotFound(ex.Message),
                _ => Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError)
            };
        }
    });

app.Run();