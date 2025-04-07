using CovrMe.WebAPI.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Users
{
    public class CheckResetPasswordCodeInput
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
