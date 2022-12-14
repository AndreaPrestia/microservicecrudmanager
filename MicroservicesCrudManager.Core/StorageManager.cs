using System.Security.Claims;
using MicroservicesCrudManager.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesCrudManager.Core;

public sealed class StorageManager
{
    private readonly IServiceProvider _serviceProvider;

    public StorageManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? ActivateAdd(string entity, object? payload, ClaimsPrincipal claimsPrincipal)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        if (payload == null)
        {
            throw new ArgumentNullException($"No payload provided in /api/v1/{entity} POST");
        }

        var entityType = GetEntityType(entity);

        if (entityType == null)
        {
            throw new InvalidOperationException($"Entity {entity} invalid");
        }

        var addType = GetAddType(entityType);

        if (addType == null)
        {
            throw new InvalidOperationException($"No IAdd<{entity}> implementation found");
        }

        var service = GetService(addType);

        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        var roles = GetPropertyValue(service, "Roles");

        if (roles != null)
        {
            if (!claimsPrincipal.IsInRole(roles.ToString()))
            {
                throw new UnauthorizedAccessException($"Not authorized to entity {entity} Add");
            }
        }

        var addMethod = service.GetType().GetMethods().Where(t => t.Name.Equals("Add")
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

        var result = addMethod.Invoke(service, new[] { payload });

        return result;
    }

    public object? ActivateUpdate(string entity, string id, object? payload, ClaimsPrincipal claimsPrincipal)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException($"No id provided in /api/v1/{entity}/?id= PUT");
        }

        if (payload == null)
        {
            throw new ArgumentNullException($"No payload provided in /api/v1/{entity}/?id= PUT");
        }

        var entityType = GetEntityType(entity);

        if (entityType == null)
        {
            throw new InvalidOperationException($"Entity {entity} invalid");
        }

        var type = GetUpdateType(entityType);

        if (type == null)
        {
            throw new InvalidOperationException($"No IUpdate<{entity}> implementation found");
        }

        var service = GetService(type);

        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        var roles = GetPropertyValue(service, "Roles");

        if (roles != null)
        {
            if (!claimsPrincipal.IsInRole(roles.ToString()))
            {
                throw new UnauthorizedAccessException($"Not authorized to entity {entity} Update");
            }
        }

        var method = service.GetType().GetMethods().Where(t => t.Name.Equals("Update")
                                                               && t.ReturnParameter.ParameterType.Name
                                                                   .Equals(
                                                                       entity)
                                                               && t.GetParameters().Length == 1 &&
                                                               t.GetParameters().FirstOrDefault()!
                                                                   .ParameterType
                                                                   .Name.Equals(entity)
            ).Select(x => x)
            .FirstOrDefault();

        if (method == null)
        {
            throw new EntryPointNotFoundException(
                $"No implementation of method {entity} Update({entity} entity)");
        }

        var result = method.Invoke(method, new[] { id, payload });

        return result;
    }

    public object? ActivateGet(string entity, string id, ClaimsPrincipal claimsPrincipal)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException($"No id provided in /api/v1/{entity}/?id= PUT");
        }

        var entityType = GetEntityType(entity);

        if (entityType == null)
        {
            throw new InvalidOperationException($"Entity {entity} invalid");
        }

        var type = GetGetType(entityType);

        if (type == null)
        {
            throw new InvalidOperationException($"No IGet<{entity}> implementation found");
        }

        var service = GetService(type);

        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }
        
        var roles = GetPropertyValue(service, "Roles");

        if (roles != null)
        {
            if (!claimsPrincipal.IsInRole(roles.ToString()))
            {
                throw new UnauthorizedAccessException($"Not authorized to entity {entity} Updated");
            }
        }

        var method = service.GetType().GetMethods().Where(t => t.Name.Equals("Get")
                                                               && t.ReturnParameter.ParameterType.Name
                                                                   .Equals(
                                                                       entity)
                                                               && t.GetParameters().Length == 1 &&
                                                               t.GetParameters().FirstOrDefault()!
                                                                   .ParameterType
                                                                   .Name.Equals(entity)
            ).Select(x => x)
            .FirstOrDefault();

        if (method == null)
        {
            throw new EntryPointNotFoundException(
                $"No implementation of method {entity} Get({entity} entity)");
        }

        var result = method.Invoke(method, new[] { id });

        return result;
    }

    public object? ActivateList(string entity, ClaimsPrincipal claimsPrincipal, int page = 0, int limit = 10, string? query = null,
        string? orderBy = null, bool ascending = false)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var entityType = GetEntityType(entity);

        if (entityType == null)
        {
            throw new InvalidOperationException($"Entity {entity} invalid");
        }

        var type = GetListType(entityType);

        if (type == null)
        {
            throw new InvalidOperationException($"No IList<{entity}> implementation found");
        }

        var service = GetService(type);

        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        var roles = GetPropertyValue(service, "Roles");

        if (roles != null)
        {
            if (!claimsPrincipal.IsInRole(roles.ToString()))
            {
                throw new UnauthorizedAccessException($"Not authorized to entity {entity} List");
            }
        }
        
        var method = service.GetType().GetMethods().Where(t => t.Name.Equals("List")
                                                               && t.ReturnParameter.ParameterType.Name
                                                                   .Equals(
                                                                       entity)
                                                               && t.GetParameters().Length == 1 &&
                                                               t.GetParameters().FirstOrDefault()!
                                                                   .ParameterType
                                                                   .Name.Equals(entity)
            ).Select(x => x)
            .FirstOrDefault();

        if (method == null)
        {
            throw new EntryPointNotFoundException(
                $"No implementation of method List<{entity}> List({entity} entity)");
        }

        var result = method.Invoke(method, new object?[] { page, limit, query, orderBy, ascending });

        return result;
    }


    public object? ActivateDelete(string entity, string id, ClaimsPrincipal claimsPrincipal)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException($"No id provided in /api/v1/{entity}/?id= DELETE");
        }

        var entityType = GetEntityType(entity);

        if (entityType == null)
        {
            throw new InvalidOperationException($"Entity {entity} invalid");
        }

        var addType = GetDeleteType(entityType);

        if (addType == null)
        {
            throw new InvalidOperationException($"No IDelete<{entity}> implementation found");
        }

        var service = GetService(addType);

        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }
        
        var roles = GetPropertyValue(service, "Roles");

        if (roles != null)
        {
            if (!claimsPrincipal.IsInRole(roles.ToString()))
            {
                throw new UnauthorizedAccessException($"Not authorized to entity {entity} Delete");
            }
        }

        var method = service.GetType().GetMethods().Where(t => t.Name.Equals("Delete")
                                                               && t.ReturnParameter.ParameterType.Name
                                                                   .Equals(
                                                                       entity)
                                                               && t.GetParameters().Length == 1 &&
                                                               t.GetParameters().FirstOrDefault()!
                                                                   .ParameterType
                                                                   .Name.Equals(entity)
            ).Select(x => x)
            .FirstOrDefault();

        if (method == null)
        {
            throw new EntryPointNotFoundException(
                $"No implementation of method {entity} Delete({entity} entity)");
        }

        var result = method.Invoke(method, new object?[] { id });

        return result;
    }

    private Type? GetEntityType(string entity)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes()
            .FirstOrDefault(x => x.Name.Equals(entity));
    }

    private Type GetAddType(Type entityType)
    {
        var typeOfId = entityType.GetProperty("Id").PropertyType;
        
        var constructed = typeof(IAdd<,>).MakeGenericType(entityType, typeOfId);

        return AppDomain.CurrentDomain.GetAssemblies()
            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes().Where(t =>
                t.GetInterfaces().Contains(constructed) && !t.IsAbstract && !t.IsInterface).Select(t =>
                t).FirstOrDefault()!;
    }

    private Type GetListType(Type entityType)
    {
        var typeOfId = entityType.GetProperty("Id").PropertyType;

        var constructed = typeof(IList<,>).MakeGenericType(entityType, typeOfId);

        return AppDomain.CurrentDomain.GetAssemblies()
            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes().Where(t =>
                t.GetInterfaces().Contains(constructed) && !t.IsAbstract && !t.IsInterface).Select(t =>
                t).FirstOrDefault()!;
    }

    private Type GetGetType(Type entityType)
    {
        var typeOfId = entityType.GetProperty("Id").PropertyType;

        var constructed = typeof(IGet<,>).MakeGenericType(entityType, typeOfId);

        return AppDomain.CurrentDomain.GetAssemblies()
            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes().Where(t =>
                t.GetInterfaces().Contains(constructed) && !t.IsAbstract && !t.IsInterface).Select(t =>
                t).FirstOrDefault()!;
    }

    private Type GetDeleteType(Type entityType)
    {
        var typeOfId = entityType.GetProperty("Id").PropertyType;

        var constructed = typeof(IDelete<,>).MakeGenericType(entityType, typeOfId);

        return AppDomain.CurrentDomain.GetAssemblies()
            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes().Where(t =>
                t.GetInterfaces().Contains(constructed) && !t.IsAbstract && !t.IsInterface).Select(t =>
                t).FirstOrDefault()!;
    }

    private Type GetUpdateType(Type entityType)
    {
        var typeOfId = entityType.GetProperty("Id").PropertyType;

        var constructed = typeof(IUpdate<,>).MakeGenericType(entityType, typeOfId);

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
    
    private object GetPropertyValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}