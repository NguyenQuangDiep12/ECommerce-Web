namespace HelloKitty.Domain.Orders.Enums
{
    public enum RefundStatus
    {
        Pending,      // Yêu cầu hoàn tiền vừa tạo
        Approved,     // Shop duyệt hoàn tiền
        Rejected,     // Từ chối hoàn tiền
        Processing,   // Đang xử lý hoàn tiền (qua cổng thanh toán)
        Completed     // Hoàn tiền thành công
    }
}
