using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyCalculationAddressRecipientModel
    {
        [JsonProperty("privatePerson")]
        public bool PrivatePerson { get; set; } = true;

        [JsonProperty("addressLocation")]
        public SpeedyAddressLocationModel? AddressLocation { get; set; }

    }
}
