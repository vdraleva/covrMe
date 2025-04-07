using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Users.Payloads
{
    public class AddUserToFamilyAndFriendsPayload : BaseResultModel
    {
        public AddUserToFamilyAndFriendsPayload()
        {
            this.User = new UserModel();
        }
        public UserModel? User { get; set; }
    }
}
