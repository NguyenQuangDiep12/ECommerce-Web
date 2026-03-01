namespace HelloKitty.Domain.Catalog.Enums
{
    public enum ProductStatus
    {
        Draft,        // Chưa công khai
        Active,       // Đang bán
        Inactive,     // Tạm ngưng bán
        OutOfStock,   // Hết hàng
        Archived      // Ngừng kinh doanh (xóa mềm)
    }
}
