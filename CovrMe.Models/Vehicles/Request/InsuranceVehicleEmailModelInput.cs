using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Request
{
    public class InsuranceVehicleEmailModelInput
    {
        public string? VehicleBrand { get; set; }
        public string? VehicleModel { get; set; }
        public string? FirstRegistrationDate { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleUsage { get; set; }
        public int EngineVolume { get; set; }
        public int BatteryCapacity { get; set; }
        public int EngineType { get; set; }
        public string? EngineTypeText { get; set; }
        public int VehicleKilowatts { get; set; }
    }
}
