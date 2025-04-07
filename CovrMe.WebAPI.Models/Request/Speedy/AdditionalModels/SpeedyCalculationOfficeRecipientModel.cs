using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyCalculationOfficeRecipientModel
    {
        [JsonProperty("privatePerson")]
        public bool PrivatePerson { get; set; } = true;

        [JsonProperty("pickupOfficeId")]
        public int PickupOfficeId { get; set; }
    }
}
