namespace HelloKitty.Domain.Catalog.Entities
{
    public class AttributeValue
    {
        public Guid ValueId { get; set; } = Guid.NewGuid();
        public string ValueName { get; set; } = string.Empty;
        public Guid AttributeId { get; set; }
        public Attribute Attribute { get; set; } = null!;
    }
}
