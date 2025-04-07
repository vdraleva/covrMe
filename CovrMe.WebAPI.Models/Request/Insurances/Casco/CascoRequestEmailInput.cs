using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Insurances.Casco
{
    public class CascoRequestEmailInput
    {
        public InsuranceUserEmailModelInput UserInfo { get; set; }
        public InsuranceVehicleEmailModelInput VehicleInfo { get; set; }
    }
}
