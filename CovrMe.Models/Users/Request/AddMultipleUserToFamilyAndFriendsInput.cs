using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Request
{
    public class AddMultipleUserToFamilyAndFriendsInput
    {
        public AddMultipleUserToFamilyAndFriendsInput()
        {
            this.Users = new List<AddUserToFamilyAndFriendsInput>();
        }

        public List<AddUserToFamilyAndFriendsInput> Users { get; set; }
    }
}
