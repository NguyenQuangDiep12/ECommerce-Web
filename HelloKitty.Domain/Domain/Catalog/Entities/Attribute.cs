namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class Attribute
    {
        public Guid AttributeId { get; set; } = Guid.NewGuid();
        public string AttributeName { get; set; } = string.Empty;
        public List<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();
    }
}
