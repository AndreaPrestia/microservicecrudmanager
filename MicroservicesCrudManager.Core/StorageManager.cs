using MicroservicesCrudManager.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesCrudManager.Core;

public class StorageManager
{
    private readonly IServiceProvider _serviceProvider;

    public StorageManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? ActivateAdd(string entity, object payload)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var entityType = GetEntityType(entity);

        if (entityType == null)
        {
            throw new InvalidOperationException($"Entity {entity} invalid");
        }

        var addType = GetAddType(entityType);

        if (addType == null)
        {
            throw new InvalidOperationException($"No IAddEntity<entity> implementation found");
        }

        var addService = GetService(addType);

        if (addService == null)
        {
            throw new ArgumentNullException(nameof(addService));
        }

        var addMethod = addService.GetType().GetMethods().Where(t => t.Name.Equals("Add")
                                                                     && t.ReturnParameter.ParameterType.Name.Equals(
                                                                         entity)
                                                                     && t.GetParameters().Length == 1 &&
                                                                     t.GetParameters().FirstOrDefault()!.ParameterType
                                                                         .Name.Equals(entity)
            ).Select(x => x)
            .FirstOrDefault();

        if (addMethod == null)
        {
            throw new EntryPointNotFoundException($"No implementation of method {entity} Add({entity} entity)");
        }

        var result = addMethod.Invoke(addService, new object?[] { payload });

        return result;
    }

    private Type? GetEntityType(string entity)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes()
            .FirstOrDefault(x => x.Name.Equals(entity));
    }

    private Type? GetAddType(Type entityType)
    {
        var constructed = typeof(IAddEntity<>).MakeGenericType(entityType);

        return AppDomain.CurrentDomain.GetAssemblies()
            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes().Where(t =>
                t.GetInterfaces().Contains(constructed) && !t.IsAbstract && !t.IsInterface).Select(t =>
                t).FirstOrDefault()!;
    }

    private object? GetService(Type type)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var provider = serviceScope.ServiceProvider;

        return provider.GetService(type);
    }
}