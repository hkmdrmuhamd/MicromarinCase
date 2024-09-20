namespace MicromarinCase.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Dictionary<string, object> DynamicFields { get; set; } = new Dictionary<string, object>();
    }
}
