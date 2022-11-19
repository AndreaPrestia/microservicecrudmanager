namespace MicroservicesCrudManager.Core.Interfaces;

public interface IUpdateEntity<T> : IEntityStorageManager<T> where T : class, new()
{
    T Update(T entity);
}