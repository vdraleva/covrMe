using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.City
{
    public class GetCitiesResultModel
    {
        public GetCitiesResultModel()
        {
            this.Cities = new List<SirmaCityModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<SirmaCityModel> Cities { get; set; }
    }
}
