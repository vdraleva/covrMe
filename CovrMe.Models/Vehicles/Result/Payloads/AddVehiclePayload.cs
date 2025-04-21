using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result.Payloads
{
    public class AddVehiclePayload
    {
        public AddVehiclePayload()
        {
            this.Result = new AddVehicleResultModel();
        }
        [JsonProperty("addVehicle")]
        public AddVehicleResultModel? Result { get; set; }
    }
}
