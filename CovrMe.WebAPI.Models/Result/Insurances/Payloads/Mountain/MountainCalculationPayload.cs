using CovrMe.WebAPI.Models.Result.Insurances.Mountain;
using CovrMe.WebAPI.Models.Result.Insurances.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads.Mountain
{
    public class MountainCalculationPayload
    {
        public MountainCalculationPayload()
        {
            CalculationResult = new List<MountainCalculationResultModel>();
        }
        public List<MountainCalculationResultModel>? CalculationResult { get; set; }
    }
}
