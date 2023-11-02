namespace MicroServicesCrudManager.Core.Interfaces;

public interface IUpdate<T, T1> : IEntityStorageManager<T, T1> where T : BaseEntity<T1>
{
    T Update(T1 id, T entity);
}