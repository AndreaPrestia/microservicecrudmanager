namespace MicroServicesCrudManager.Core;

public abstract class BaseEntity<T>
{
    public T? Id { get; set; }
}