using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Deliveries.Result
{
    public class SpeedyCalculationResultModel
    {
        public SpeedyCalculationResultModel()
        {
            this.Calculations = new List<SpeedyPriceResultModel>();
        }

        [JsonProperty("calculations")]
        public List<SpeedyPriceResultModel> Calculations { get; set; }
    }
}
