namespace MicroservicesCrudManager.Core.Interfaces;

public interface IAdd<T, T1> : IEntityStorageManager<T, T1> where T : IHasId<T1>,  new()
{
    T Add(T entity);
}