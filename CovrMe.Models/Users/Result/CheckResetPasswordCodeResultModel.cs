using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result
{
    public class CheckResetPasswordCodeResultModel : BaseResultModel
    {
        public bool IsCodeValid { get; set; }
    }
}
