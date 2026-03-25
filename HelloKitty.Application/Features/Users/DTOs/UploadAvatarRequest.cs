using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Users.DTOs
{
    public class UploadAvatarRequest
    {
        public Guid UserId { get; set; }
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
    }
}
