using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyOfficeShipmentRecipientModel
    {
        [JsonProperty("phone1")]
        public SpeedyPhoneModel? PhoneNumber { get; set; }

        [JsonProperty("clientName")]
        public string? Names { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("privatePerson")]
        public bool PrivatePerson { get; set; } = true;

        [JsonProperty("pickupOfficeId")]
        public int PickupOfficeId { get; set; }
    }
}
