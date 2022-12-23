namespace MicroservicesCrudManager.Core.Interfaces;

public interface IDelete<T, T1> : IEntityStorageManager<T, T1> where T : BaseEntity<T1>
{
    void Delete(T1 id);
}