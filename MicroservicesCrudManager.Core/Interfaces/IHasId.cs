namespace MicroservicesCrudManager.Core.Interfaces;

public interface IHasId<T>
{
    T Id { get; set; }
}