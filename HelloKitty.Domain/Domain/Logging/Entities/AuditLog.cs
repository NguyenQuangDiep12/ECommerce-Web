using HelloKitty.API.Domain.Logging.Enums;

namespace HelloKitty.API.Domain.Logging.Entities
{
    /// <summary>
    /// Ghi lai thao tac nguoi dung/admin len du lieu (create, delete, update, ....)
    /// 
    /// UserId la loose reference (khong co FK constraint , khong co EF navigation) vi:
    /// 1. AuditLog la du lieu lich su bat bien - phai ton tai ke ca khi user bi xoa mem.
    /// 2. Khong can load user khi bi query log, truy van theo userid qua AuditLog Repository.
    /// </summary>
    public class AuditLog
    {
        public Guid AuditLogId { get; set; } = Guid.NewGuid();
        // Ai la nguoi thuc hien 
        public Guid? UserId { get; set; }
        public string? UserEmail { get; set; }

        // thuc hien gi, tren bang nao, ban ghi nao
        public AuditAction AuditAction { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string RecordId { get; set; } = string.Empty; // primary key cua ban ghi bi tac dong 
        // du lieu thay doi
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        // thong tin request
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? EndPoint { get; set; }

        public DateTime CreatedAt { get; private set; }
    }
}
