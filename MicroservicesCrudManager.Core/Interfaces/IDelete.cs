namespace MicroservicesCrudManager.Core.Interfaces;

public interface IDelete<T, T1> : IEntityStorageManager<T, T1> where T : IHasId<T1>, new()
{
    void Delete(T1 id);
}