using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsuranceInstallment
    {
        public decimal PremiumWithTax { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
