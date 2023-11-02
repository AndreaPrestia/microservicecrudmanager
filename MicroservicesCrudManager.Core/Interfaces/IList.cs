namespace MicroServicesCrudManager.Core.Interfaces;

public interface IList<T, T1> : IEntityStorageManager<T, T1> where T : BaseEntity<T1>
{
    IEnumerable<T> List(int page = 0, int limit = 10, string? query = null, string? orderBy = null, bool ascending = false);
}