using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result
{
    public class GetUserVehiclesResultModel
    {
        public GetUserVehiclesResultModel()
        {
            this.Vehicles = new List<VehicleResultModel>();
        }
        [JsonProperty("nodes")]
        public List<VehicleResultModel> Vehicles { get; set; }
    }
}
