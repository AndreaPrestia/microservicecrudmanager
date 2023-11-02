using MicroServicesCrudManager.Core;
using MicroServicesCrudManager.Core.Attributes;

namespace WebApi.Entities;

[EntityName("entities")]
public class TestEntity : BaseEntity<string>
{
    public string Name { get; set; } = null!;
}