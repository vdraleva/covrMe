using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Speedy.Payloads
{
    public class FindOfficePayload
    {
        public FindOfficePayload()
        {
            this.Offices = new List<SpeedyOfficeModel>();
        }

        [JsonProperty("offices")]
        public List<SpeedyOfficeModel>? Offices { get; set; }
    }
}
