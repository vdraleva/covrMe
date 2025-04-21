using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result
{
    public class EngineOcrTypesResultModel
    {
        public EngineOcrTypesResultModel()
        {
            this.EngineTypes = new List<BaseOcrDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<BaseOcrDataModel> EngineTypes { get; set; }
    }
}
