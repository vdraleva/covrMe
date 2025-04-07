using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class InstallmentModel
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalWithoutTax { get; set; }
        public decimal GovernmentFunds { get; set; }
    }
}
