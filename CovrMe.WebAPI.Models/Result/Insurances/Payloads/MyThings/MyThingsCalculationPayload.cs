using CovrMe.WebAPI.Models.Result.Insurances.Health;
using CovrMe.WebAPI.Models.Result.Insurances.MyThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads.MyThings
{
    public class MyThingsCalculationPayload
    {
        public MyThingsCalculationPayload()
        {
            CalculationResult = new List<MyThingsCalculationResultModel>();
        }
        public List<MyThingsCalculationResultModel>? CalculationResult { get; set; }
    }
}
