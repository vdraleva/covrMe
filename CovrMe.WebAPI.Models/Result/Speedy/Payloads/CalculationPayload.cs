using CovrMe.WebAPI.Models.Request.Speedy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Speedy.Payloads
{
    public class CalculationPayload
    {
        public CalculationPayload()
        {
            this.Calculations = new List<SpeedyPriceResultModel>();
        }

        [JsonProperty("calculations")]
        public List<SpeedyPriceResultModel> Calculations { get; set; }
    }
}
