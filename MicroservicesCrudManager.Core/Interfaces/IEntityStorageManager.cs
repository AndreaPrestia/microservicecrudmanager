namespace MicroservicesCrudManager.Core.Interfaces;

public interface IEntityStorageManager<T, T1> where T : IHasId<T1>, new()
{
}