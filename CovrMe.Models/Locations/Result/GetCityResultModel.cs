using CovrMe.Models.Vehicles.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Locations.Result
{
    public class GetCityResultModel
    {
        public GetCityResultModel()
        {
            this.Cities = new List<CityDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<CityDataModel> Cities { get; set; }
    }
}
