using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroServicesCrudManager.Core;

public static class Extensions
{
    public static void AddMcm(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<StorageManager>();
    }

    public static void MapMcmEndpoints(this WebApplication app)
    {
        ApiRouter.Init(app);
    }
}