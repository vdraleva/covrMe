using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result
{
    public class GetUserByIdResultModel
    {
        [JsonProperty("nodes")]
        public List<UserModel> User { get; set; }
    }
}
