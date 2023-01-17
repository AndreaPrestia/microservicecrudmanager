using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesCrudManager.Core;

public static class Extensions
{
    public static WebApplicationBuilder? AddMcm(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<StorageManager>();

        return builder;
    }

    public static WebApplication MapMcmEndpoints(this WebApplication app)
    {
        ApiRouter.Init(app);
        return app;
    }
}