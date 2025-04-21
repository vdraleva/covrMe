using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.TravelInsurance
{
    public class TravelCalculationResultModel
    {
        public TravelCalculationResultModel()
        {
            this.Offers = new List<CalculationModel>();
        }

        [JsonProperty("calculationResult")]
        public List<CalculationModel>? Offers { get; set; }
    }
}
