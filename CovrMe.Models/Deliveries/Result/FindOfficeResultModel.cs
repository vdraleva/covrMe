using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Deliveries.Result
{
    public class FindOfficeResultModel
    {
        public FindOfficeResultModel()
        {
            Offices = new List<SpeedyOfficeModel>();
        }

        [JsonProperty("offices")]
        public List<SpeedyOfficeModel>? Offices { get; set; }
    }
}
