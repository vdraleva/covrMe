using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Vehicles.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.Casco
{
    public class CascoRequestEmailInput
    {
        public InsuranceVehicleEmailModelInput VehicleInfo { get; set; }
        public InsuranceUserEmailModelInput UserInfo { get; set; }
    }
}
