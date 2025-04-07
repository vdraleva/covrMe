using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.Municipalities
{
    public class GetMunicipalitiesResultModel
    {
        public GetMunicipalitiesResultModel()
        {
            this.Municipalities = new List<SirmaBaseDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<SirmaBaseDataModel> Municipalities { get; set; }
    }
}
