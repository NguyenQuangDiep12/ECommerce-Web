namespace HelloKitty.Domain.Catalog.Entities
{
    public class VariantAttribute
    {
        public Guid VariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;
        public Guid ValueId { get; set; }
        public AttributeValue AttributeValue { get; set; } = null!;
    }
}
