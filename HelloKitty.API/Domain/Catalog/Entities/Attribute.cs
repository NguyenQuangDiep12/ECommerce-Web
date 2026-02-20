namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class Attribute
    {
        public Guid AttributeId { get; set; } = Guid.NewGuid();
        public string AttributeName { get; set; } = string.Empty;
        public List<AttributeValue> attributeValues { get; set; } = new List<AttributeValue>();
    }
}
