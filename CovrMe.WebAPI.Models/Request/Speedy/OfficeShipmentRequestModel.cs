using CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy
{
    public class OfficeShipmentRequestModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("recipient")]
        public SpeedyOfficeShipmentRecipientModel? Recipient { get; set; }

        [JsonProperty("service")]
        public SpeedyServiceModel? Service { get; set; }

        [JsonProperty("content")]
        public SpeedyContentModel? Content { get; set; }

        [JsonProperty("payment")]
        public SpeedyPaymentModel? Payment { get; set; }
    }
}
