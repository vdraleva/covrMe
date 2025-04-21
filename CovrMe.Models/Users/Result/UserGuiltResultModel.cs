using CovrMe.Models.Vehicles.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result
{
    public class UserGuiltResultModel
    {
        public UserGuiltResultModel()
        {
            this.Guilts = new List<BaseDataModel>();
        }

        [JsonProperty("result")]
        public List<BaseDataModel> Guilts { get; set; }
    }
}
