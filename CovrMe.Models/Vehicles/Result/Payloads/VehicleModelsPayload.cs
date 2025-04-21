using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result.Payloads
{
    public class VehicleModelsPayload
    {
        public VehicleModelsPayload()
        {
            this.Result = new VehicleModelsResultModel();
        }

        [JsonProperty("vehicleModels")]
        public VehicleModelsResultModel? Result { get; set; }
    }
}
