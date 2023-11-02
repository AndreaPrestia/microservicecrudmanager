namespace MicroServicesCrudManager.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class EntityNameAttribute : Attribute
    {
        public string Name { get; }

        public EntityNameAttribute(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            
            Name = name;
        }
    }
}
