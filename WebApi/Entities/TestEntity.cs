using MicroservicesCrudManager.Core;

namespace WebApi.Entities;

public class TestEntity : BaseEntity<string>
{
    public string Name { get; set; }
}