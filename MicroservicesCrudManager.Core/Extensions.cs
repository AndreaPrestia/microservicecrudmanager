using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesCrudManager.Core;

public static class Extensions
{
    public static WebApplicationBuilder? AddMscm(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<StorageManager>();

        return builder;
    }

    public static WebApplication MapMscmEndpoints(this WebApplication app)
    {
        new ApiRouter().Init(app);
        return app;
    }
}