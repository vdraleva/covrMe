using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class InsuranceChartData
    {
        public List<string>? Labels { get; set; }
        public List<int>? Total { get; set; }
        public List<decimal>? DecimalTotals { get; set; }
    }
}
