using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Deliveries.Result.Payloads
{
    public class FindOfficePayload
    {
        [JsonProperty("findOffice")]
        public FindOfficeResultModel SpeedyOffices { get; set; }
    }
}
