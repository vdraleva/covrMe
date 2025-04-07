using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Users
{
    public class DeleteUserInput
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
    }
}
