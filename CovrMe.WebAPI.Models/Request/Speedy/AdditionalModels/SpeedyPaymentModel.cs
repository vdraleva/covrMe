using CovrMe.WebAPI.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyPaymentModel
    {
        [JsonProperty("courierServicePayer")]
        public string? CourierServicePayer { get; set; } = GlobalConstants.CourierServicePayer;
    }
}
