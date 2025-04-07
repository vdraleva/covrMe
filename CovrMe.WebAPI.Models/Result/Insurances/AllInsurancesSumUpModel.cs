using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class AllInsurancesSumUpModel
    {
        public decimal TotalInsurancesPrice { get; set; }
        public int TotalInsurances { get; set; }
        public int TotalInsurers { get; set; }
        public InsuranceChartData? InsuranceChartData { get; set; }
    }
}
