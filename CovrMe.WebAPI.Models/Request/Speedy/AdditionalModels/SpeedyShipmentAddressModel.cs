using CovrMe.WebAPI.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyShipmentAddressModel
    {
        [JsonProperty("countryId")]
        public int CountryId { get; set; } = GlobalConstants.CountryId;

        [JsonProperty("postCode")]
        public string PostCode { get; set; }

        [JsonProperty("streetNo")]
        public string? StreetNo { get; set; }

        [JsonProperty("blockNo")]
        public string? BlockNo { get; set; }

        [JsonProperty("entranceNo")]
        public string? EntranceNo { get; set; }

        [JsonProperty("floorNo")]
        public string? FloorNo { get; set; }

        [JsonProperty("apartmentNo")]
        public string? ApartmentNo { get; set; }

        [JsonProperty("addressNote")]
        public string? AddressNote { get; set; }
    }
}
