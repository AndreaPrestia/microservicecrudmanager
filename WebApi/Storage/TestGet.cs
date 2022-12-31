using MicroservicesCrudManager.Core.Interfaces;
using WebApi.Entities;

namespace WebApi.Storage;

public class TestGet : IGet<TestEntity, string>
{
    public string Roles { get; }
    
    public TestEntity Get(string id)
    {
        return new TestEntity()
        {
            Id = id,
            Name = "Test"
        };
    }
}