using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result.Payloads
{
    public class ChangeUserPasswordPayload
    {
        [JsonProperty("changeUserPassword")]
        public BaseResultModel? Result { get; set; }
    }
}
