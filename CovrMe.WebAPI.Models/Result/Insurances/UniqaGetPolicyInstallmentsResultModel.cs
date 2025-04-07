using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class UniqaGetPolicyInstallmentsResultModel
    {
        public string? InsuredUser { get; set; }
        public InstallmentModel? Installment { get; set; }
    }
}
