using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Shared.Enums
{
    public enum InsuranceTypeEnum
    {
        [Description("Гражданска отговорност")]
        Civil = 0,

        [Description("Помощ при пътуване")]
        Travel = 1,

        [Description("Здраве и спокойствие")]
        Health = 2,

        [Description("Планинска застраховка")]
        Mountain = 3,

        [Description("Моите вещи")]
        MyThings = 4
    }
}
