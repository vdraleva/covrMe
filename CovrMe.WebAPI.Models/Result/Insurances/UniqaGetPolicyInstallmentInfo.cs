using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class UniqaGetPolicyInstallmentInfo
    {
        public int AttachmentNumber { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime? InstallmentDate { get; set; }
        public decimal Sum { get; set; }
    }
}
