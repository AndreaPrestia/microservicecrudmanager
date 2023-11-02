namespace MicroServicesCrudManager.Core.Interfaces;

public interface IGet<T, T1> : IEntityStorageManager<T, T1> where T : BaseEntity<T1>
{
    T Get(T1 id);
}