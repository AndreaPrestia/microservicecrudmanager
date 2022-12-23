namespace MicroservicesCrudManager.Core.Interfaces;

public interface IUpdate<T, T1> : IEntityStorageManager<T, T1> where T : IHasId<T1>,  new()
{
    T Update(T1 id, T entity);
}