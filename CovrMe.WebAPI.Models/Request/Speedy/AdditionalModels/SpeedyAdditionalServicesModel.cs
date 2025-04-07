using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyAdditionalServicesModel
    {
        [JsonProperty("obpd")]
        public SpeedyObpdModel? Obpd { get; set; }
    }
}
