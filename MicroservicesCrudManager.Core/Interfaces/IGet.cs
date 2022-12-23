namespace MicroservicesCrudManager.Core.Interfaces;

public interface IGet<T, T1> : IEntityStorageManager<T, T1> where T : IHasId<T1>, new()
{
    T Get(T1 id);
}