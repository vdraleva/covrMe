using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result.Payloads
{
    public class EditVehiclePayload
    {
        public EditVehiclePayload()
        {
            this.Result = new EditVehicleResultModel();
        }
        [JsonProperty("editVehicle")]
        public EditVehicleResultModel? Result { get; set; }
    }
}
