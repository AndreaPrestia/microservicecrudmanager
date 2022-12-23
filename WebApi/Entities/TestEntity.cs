using MicroservicesCrudManager.Core.Interfaces;

namespace WebApi.Entities;

public class TestEntity : IHasId<string>
{
    public string Id { get; set; }
    public string Name { get; set; }
}