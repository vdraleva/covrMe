using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Shared.Enums
{
    public enum RolesEnum
    {
        [Description("Администратор")]
        Administrator,
        [Description("Потребител")]
        User
    }
}
