using CovrMe.WebAPI.Models.Result.Speedy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy
{
    public class SpeedyPriceResultModel
    {
        [JsonProperty("price")]
        public SpeedyPriceModel? Price { get; set; }
    }
}
