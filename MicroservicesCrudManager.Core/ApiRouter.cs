using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace MicroservicesCrudManager.Core;

internal sealed class ApiRouter
{
    internal static void Init(WebApplication app)
    {
        using var loggerFactory = LoggerFactory.Create(configure =>
        {
            configure.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
        });

        var logger = loggerFactory.CreateLogger<ApiRouter>();

        app.MapGet("/", () => "Mcm is up and running :)");

        app.MapPost("/api/v1/{entity}",
            (HttpContext httpContext, StorageManager storageManager, string entity) =>
            {
                try
                {
                    var result =
                        storageManager.ActivateAdd(entity, httpContext.Request.Body.Deserialize(entity),
                            httpContext.User);

                    return Results.Created($"/api/v1/{entity}", result);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);

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
                    var result = storageManager.ActivateUpdate(entity, id, httpContext.Request.Body.Deserialize(entity),
                        httpContext.User);

                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);

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
                    var result = storageManager.ActivateDelete(entity, id, httpContext.User);

                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);

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
                    var result = storageManager.ActivateGet(entity, id, httpContext.User);

                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);

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
                    var result = storageManager.ActivateList(entity, httpContext.User, page, limit, query, orderBy,
                        ascending);

                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);

                    return ex switch
                    {
                        ArgumentException => Results.BadRequest(ex.Message),
                        EntryPointNotFoundException => Results.NotFound(ex.Message),
                        _ => Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError)
                    };
                }
            });
    }
}