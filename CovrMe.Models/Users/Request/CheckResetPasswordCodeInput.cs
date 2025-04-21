using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Request
{
    public class CheckResetPasswordCodeInput
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
