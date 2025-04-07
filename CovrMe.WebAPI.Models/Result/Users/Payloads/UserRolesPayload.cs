using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Users.Payloads
{
    public class UserRolesPayload
    {
        public UserRolesPayload()
        {
            this.Roles = new List<string>();
        }
        public ICollection<string> Roles { get; set; }
    }
}
