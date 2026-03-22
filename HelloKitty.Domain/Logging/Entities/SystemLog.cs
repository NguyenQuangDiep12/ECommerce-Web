using HelloKitty.Domain.Logging.Enums;
using HelloKitty.Domain.Users.Entities;

namespace HelloKitty.Domain.Logging.Entities
{
    /// <summary>
    /// Ghi lai cac su kien cap he thong: loi runtime, canh bao
    /// </summary>
    public class SystemLog
    {
        public Guid LogId { get; set; } = Guid.NewGuid();
        public LogLevel LogLevel { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        // Chi tiet loi
        public string? RequestPath { get; set; } // vd: "/api/orders"
        public string? RequestMethod { get; set; }
        public string? IpAddress { get; set; }
        public Guid? UserId { get; set; } // user dang thuc hien request

        // thong tin bo sung dang json (correlation id , extra context, v.v..)
        public string? MetaData { get; set; }
        public DateTime CreatedAt { get; private set; }
    }
}
