using HelloKitty.Domain.Promotions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Vouchers.DTOs
{
    public class CreateVoucherRequest
    {
    public string Code { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal MinOrderAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public int MaxUsage {  get; set; }
    public int MaxUsagePerUser { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    }
}
