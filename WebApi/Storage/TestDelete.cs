using MicroservicesCrudManager.Core.Interfaces;
using WebApi.Entities;

namespace WebApi.Storage;

public class TestDelete : IDelete<TestEntity, string>
{
    public string Roles { get; }
    public void Delete(string id)
    {
        Console.WriteLine($"Delete entity {id}");
    }
}