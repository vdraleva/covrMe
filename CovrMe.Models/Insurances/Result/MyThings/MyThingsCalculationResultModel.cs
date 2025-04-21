using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.MyThings
{
    public class MyThingsCalculationResultModel
    {
        public MyThingsCalculationResultModel()
        {
            this.Offers = new List<CalculationModel>();
        }

        [JsonProperty("calculationResult")]
        public List<CalculationModel>? Offers { get; set; }
    }
}
