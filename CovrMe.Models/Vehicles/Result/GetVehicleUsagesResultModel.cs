using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result
{
    public class GetVehicleUsagesResultModel
    {
        public GetVehicleUsagesResultModel()
        {
            this.Usages = new List<BaseDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<BaseDataModel> Usages { get; set; }
    }
}
