namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class VariantAttribute
    {
        public Guid VariantId { get; set; }
        public ProductVariant productVariant { get; set; } = null!;
        public Guid ValueId { get; set; }
        public AttributeValue attributeValue { get; set; } = null!;
    }
}
