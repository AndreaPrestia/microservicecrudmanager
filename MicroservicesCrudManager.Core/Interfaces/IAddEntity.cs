namespace MicroservicesCrudManager.Core.Interfaces;

public interface IAddEntity<T> : IEntityStorageManager<T> where T : class, new()
{
    T Add(T entity);
}