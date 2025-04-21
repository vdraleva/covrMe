using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Locations.Result
{
    public class GetMunicipalityResultModel
    {
        public GetMunicipalityResultModel()
        {
            this.Municipalities = new List<LocationDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<LocationDataModel> Municipalities { get; set; }
    }
}
