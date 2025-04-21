using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Request
{
    public class EditMultipleFamilyFriendsUserInput
    {
        public EditMultipleFamilyFriendsUserInput()
        {
            this.Users = new List<EditUserInfoInput>();
        }

        public List<EditUserInfoInput> Users { get; set; }
    }
}
