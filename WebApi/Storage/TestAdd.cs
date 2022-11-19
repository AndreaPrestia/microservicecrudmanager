using System.Text.Json;
using MicroservicesCrudManager.Core.Interfaces;
using WebApi.Entities;

namespace WebApi.Storage;

public class TestAdd : IAddEntity<TestEntity>
{
    public TestEntity Add(TestEntity entity)
    {
        Console.WriteLine(JsonSerializer.Serialize(entity));

        return entity;
    }
}