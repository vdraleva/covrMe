using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result.QueryResults
{
    public class GetUserByIdResultQuery
    {
        [JsonProperty("user")]
        public GetUserByIdResultModel User { get; set; }
    }
}
