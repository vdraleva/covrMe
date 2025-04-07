using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyServiceModel
    {
        public SpeedyServiceModel()
        {
            ServiceIds = new int[] { 505 };
            this.Service = "505";
        }

        [JsonProperty("serviceId")]
        public string? Service { get; set; }
        [JsonProperty("autoAdjustPickupDate")]
        public bool AutoAdjustPickupDate { get; set; } = true;

        [JsonProperty("serviceIds")]
        public int[] ServiceIds { get; set; }

        [JsonProperty("additionalServices")]
        public SpeedyAdditionalServicesModel? AdditionalServices { get; set; }
    }
}
