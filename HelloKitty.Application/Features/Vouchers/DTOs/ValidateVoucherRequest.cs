using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Vouchers.DTOs
{
    public class ValidateVoucherRequest
    {
        public string Code { get; set; }
        public decimal OrderAmount { get; set; }
    }
}
