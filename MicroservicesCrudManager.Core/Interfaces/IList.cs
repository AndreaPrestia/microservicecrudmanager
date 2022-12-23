namespace MicroservicesCrudManager.Core.Interfaces;

public interface IList<T, T1> : IEntityStorageManager<T, T1> where T : BaseEntity<T1>
{
    IEnumerable<T> List(int page = 0, int rows = 10, string? filter = null, Dictionary<string, bool>? orderBy = null);
}