using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Users
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
