using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Shared.Enums
{
    public enum InsuranceCompanyEnum
    {
        [Description("ЗК ДЗИ")]
        Dzi = 0,

        [Description("ЗК Евроинс")]
        Euroins = 1,

        [Description("ЗК Дженерали")]
        Generali = 2,

        [Description("ЗК Групама")]
        Groupama = 3,

        [Description("ЗК ОЗК")]
        Ozk = 4,

        [Description("ЗК Булинс")]
        Bulins = 5,

        [Description("ЗК Булстрад")]
        Bulstrad = 6,

        [Description("ЗК УНИКА АД")]
        Uniqa = 7,

        [Description("ЗК Левинс")]
        Levins = 8
    }
}
