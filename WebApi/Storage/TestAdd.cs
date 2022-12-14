using System.Text.Json;
using MicroservicesCrudManager.Core.Interfaces;
using WebApi.Entities;

namespace WebApi.Storage;

public class TestAdd : IAdd<TestEntity, string>
{
    public string Roles { get; }
    public TestEntity Add(TestEntity entity)
    {
        Console.WriteLine(JsonSerializer.Serialize(entity));

        return entity;
    }
}