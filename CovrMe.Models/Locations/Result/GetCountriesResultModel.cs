using CovrMe.Models.Vehicles.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Locations.Result
{
    public class GetCountriesResultModel
    {
        public GetCountriesResultModel()
        {
            this.Countries = new List<LocationDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<LocationDataModel> Countries { get; set; }
    }
}
