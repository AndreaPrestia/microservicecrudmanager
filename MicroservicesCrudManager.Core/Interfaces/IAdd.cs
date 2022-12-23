namespace MicroservicesCrudManager.Core.Interfaces;

public interface IAdd<T, T1> : IEntityStorageManager<T, T1> where T : BaseEntity<T1>
{
    T Add(T entity);
}