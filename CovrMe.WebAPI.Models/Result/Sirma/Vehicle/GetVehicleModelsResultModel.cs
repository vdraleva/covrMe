using CovrMe.WebAPI.Models.Result.Sirma.Region;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.Vehicle
{
    public class GetVehicleModelsResultModel : BaseResultModel
    {
        public GetVehicleModelsResultModel()
        {
            this.Models = new List<SirmaBaseDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<SirmaBaseDataModel> Models { get; set; }
    }
}
