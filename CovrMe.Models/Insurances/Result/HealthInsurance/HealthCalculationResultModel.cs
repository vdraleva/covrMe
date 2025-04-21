using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.HealthInsurance
{
    public class HealthCalculationResultModel
    {
        public HealthCalculationResultModel()
        {
            this.Offers = new List<CalculationInstallmentsModel>();
        }

        [JsonProperty("calculationResult")]
        public List<CalculationInstallmentsModel>? Offers { get; set; }
    }
}
