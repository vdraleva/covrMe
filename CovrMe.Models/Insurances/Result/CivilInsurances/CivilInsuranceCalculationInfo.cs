using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsuranceCalculationInfo
    {
        public decimal PremiumWithTax { get; set; }

        public CivilInsuranceInstallments? Installments { get; set; }
        public CivilInsuranceFullInstallments? FullInstallmentsBreakdown { get; set; }
    }
}
