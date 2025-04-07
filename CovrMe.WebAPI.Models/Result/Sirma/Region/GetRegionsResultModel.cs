
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.Region
{
    public class GetRegionsResultModel
    {
        public GetRegionsResultModel()
        {
            this.Regions = new List<SirmaBaseDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<SirmaBaseDataModel> Regions { get; set; }
    }
}
