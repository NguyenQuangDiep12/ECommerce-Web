using HelloKitty.API.Domain.Promotions.Enums;

public class Voucher
{
    public Guid VoucherId { get; private set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal MinOrderAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }  // chỉ áp dụng cho %
    public int MaxUsage { get; set; }                // tổng số lượt dùng
    public int MaxUsagePerUser { get; set; }         // số lượt 1 user được dùng
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = false;
}
