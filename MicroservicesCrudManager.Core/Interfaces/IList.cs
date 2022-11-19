namespace MicroservicesCrudManager.Core.Interfaces;

public interface IList<T> : IEntityStorageManager<T> where T : class, new()
{
    IEnumerable<T> List(string? filter = null, string? order = null, int? direction = null);
}