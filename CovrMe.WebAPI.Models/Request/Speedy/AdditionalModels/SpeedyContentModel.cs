using CovrMe.WebAPI.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyContentModel
    {
        [JsonProperty("parcelsCount")]
        public int ParcelsCount { get; set; } = GlobalConstants.ParcelsCount;

        [JsonProperty("totalWeight")]
        public double TotalWeight { get; set; } = GlobalConstants.TotalWeight;

        [JsonProperty("documents")]
        public bool Documents { get; set; } = true;

        [JsonProperty("contents")]
        public string Contents { get; set; } = GlobalConstants.Contents;

        [JsonProperty("package")]
        public string Package { get; set; } = GlobalConstants.Package;
    }
}
