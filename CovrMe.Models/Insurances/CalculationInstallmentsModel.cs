using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class CalculationInstallmentsModel
    {
        public CalculationInstallmentsModel()
        {
            this.Installments = new List<InstallmentModel>();
        }
        public string? Id { get; set; }
        public decimal PremiumWithoutTax { get; set; }
        public decimal Tax { get; set; }
        public decimal Premium { get; set; }
        public string? CompanyName { get; set; }
        public string? ErrorId { get; set; }
        public string? Message { get; set; }
        public List<InstallmentModel>? Installments { get; set; }
    }
}
