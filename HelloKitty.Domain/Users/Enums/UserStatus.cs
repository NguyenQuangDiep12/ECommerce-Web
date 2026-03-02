namespace HelloKitty.Domain.Users.Enums
{
    public enum UserStatus
    {
        Active = 1, // Được đăng nhập và sử dụng hệ thống
        Inactive = 2, // Chưa kích hoạt email
        Suspended = 3, // Tạm khóa (vi phạm nhẹ)
        Banned = 4, // Khóa vĩnh viễn
        Deleted = 5, // Xóa mềm
    }
}
