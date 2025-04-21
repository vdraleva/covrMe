using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result
{
    public class GetVehicleOcrTypesResultModel
    {
        public GetVehicleOcrTypesResultModel()
        {
            this.Types = new List<BaseOcrDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<BaseOcrDataModel> Types { get; set; }
    }
}
