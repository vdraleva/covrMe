using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Payments.Request
{
    public class CreatePaymentInput
    {
        public decimal Amount { get; set; }
        public string? UserId { get; set; }
        public string? VehiclePlateNumber { get; set; }
        public string? InsuranceCompanyName { get; set; }
        public int InsuranceType { get; set; }
    }
}
