using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Users
{
    public class AddMultipleUserToFamilyAndFriendsInput
    {
        public AddMultipleUserToFamilyAndFriendsInput()
        {
            this.Users = new List<AddUserToFamilyAndFriendsInput>();
        }

        public List<AddUserToFamilyAndFriendsInput>  Users { get; set; }
    }
}
