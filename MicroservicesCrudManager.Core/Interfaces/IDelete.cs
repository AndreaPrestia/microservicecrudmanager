namespace MicroservicesCrudManager.Core.Interfaces;

public interface IDelete<T> : IEntityStorageManager<T> where T : class, new()
{
    void Delete(T entity);
}