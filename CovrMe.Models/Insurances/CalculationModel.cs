using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class CalculationModel
    {
        public string? CompanyName { get; set; }
        public decimal PremiumWithoutTax { get; set; }
        public decimal Tax { get; set; }
        public decimal Premium { get; set; }
        public string? ErrorId { get; set; }
        public string? Message { get; set; }
    }
}
