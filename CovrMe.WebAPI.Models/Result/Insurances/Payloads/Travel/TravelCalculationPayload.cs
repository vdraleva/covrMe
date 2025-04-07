using CovrMe.WebAPI.Models.Result.Insurances.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads.Travel
{
    public class TravelCalculationPayload
    {
        public TravelCalculationPayload()
        {
            this.CalculationResult = new List<TravelCalculationResultModel>();
        }
        public List<TravelCalculationResultModel>? CalculationResult { get; set; }
    }
}
