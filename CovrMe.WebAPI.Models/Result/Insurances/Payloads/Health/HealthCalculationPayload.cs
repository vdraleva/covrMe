using CovrMe.WebAPI.Models.Result.Insurances.Health;
using CovrMe.WebAPI.Models.Result.Insurances.Mountain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads.Health
{
    public class HealthCalculationPayload
    {
        public HealthCalculationPayload()
        {
            CalculationResult = new List<HealthCalculationResultModel>();
        }
        public List<HealthCalculationResultModel>? CalculationResult { get; set; }
    }
}
