using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result
{
    public class BaseInsuranceSearchModel
    {
        public string? LogoSrc { get; set; }
        public decimal Premium { get; set; }
        public decimal Tax { get; set; }
        public decimal PremiumWithoutTax { get; set; }
        public string? PremiumFormatted { get; set; }
        public string? TaxFormatted { get; set; }
        public string? PremiumWithoutTaxFormatted { get; set; }
        public string? InsuranceCompanyName { get; set; }
    }
}
