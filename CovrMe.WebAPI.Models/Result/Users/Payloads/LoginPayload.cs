using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Users.Payloads
{
    public class LoginPayload : BaseResultModel
    {
        public string? Jwt { get; set; }
        public UserModel? User { get; set; }
    }
}
