using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MicroservicesCrudManager.Core;

public static class JsonDeserializer
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        WriteIndented = true,
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true
    };

    public static object? Deserialize(this Stream stream, string entity)  
    {
        var entityType = AppDomain.CurrentDomain.GetAssemblies()
            .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes()
            .FirstOrDefault(x => x.Name.Equals(entity));

        if (entityType == null)
        {
            throw new InvalidOperationException($"Entity {entity} invalid");
        }

        var json = new StreamReader(stream).ReadToEndAsync().Result;

        var deserialized = JsonSerializer.Deserialize(json, entityType, Options);

        if (deserialized == null)
        {
            throw new ArgumentNullException($"Invalid payload for entity {entity}");
        }
        
        Validator.ValidateObject(deserialized, new ValidationContext(deserialized));
        
        return deserialized;
    }
}