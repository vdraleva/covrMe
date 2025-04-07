using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Insurances
{
    public class PayUniqaInstallmentRequestModel
    {
        public int InstallmentNumber { get; set; }
        public string? ReceiptPayer { get; set; }
        public string? ReceiptUserCreated { get; set; }
        public string? PolicyNumber { get; set; }
        public bool InsuranceGroup { get; set; }
        public string? InsuranceType { get; set; }
        public int AttachmentNumber { get; set; }
    }
}
