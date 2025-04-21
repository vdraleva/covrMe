using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Locations.Result
{
    public class GetRegionsByCountryResultModel
    {
        [JsonProperty("nodes")]
        public List<RegionModel> Regions { get; set; }
    }
}
