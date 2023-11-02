using System.Text.Json;
using MicroServicesCrudManager.Core.Interfaces;
using WebApi.Entities;

namespace WebApi.Storage;

public class TestAdd : IAdd<TestEntity, string>
{
    public TestEntity Add(TestEntity entity)
    {
        Console.WriteLine(JsonSerializer.Serialize(entity));

        return entity;
    }
}