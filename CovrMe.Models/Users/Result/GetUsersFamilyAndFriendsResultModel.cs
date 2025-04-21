using CovrMe.Models.Locations.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result
{
    public class GetUsersFamilyAndFriendsResultModel
    {
        [JsonProperty("nodes")]
        public List<UserModel> FamilyAndFriends { get; set; }
    }
}
