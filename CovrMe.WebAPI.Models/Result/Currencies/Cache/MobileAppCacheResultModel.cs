using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Currencies.Cache
{
    public class MobileAppCacheResultModel : BaseResultModel
    {
        public string Countries { get; set; }
        public string Regions { get; set; }
        public string VehicleBrands { get; set; }
    }
}
