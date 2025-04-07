using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Vehicles
{
    public class AddVehicleInput
    {
        public string? RegistrationCertificateNumber { get; set; }
        public string? VehiclePlateNumber { get; set; }
        public string? UserId { get; set; }
        public int ModelId { get; set; }
        public int BrandId { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
        public int VehicleTypeId { get; set; }
        public int VehicleUsageId { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleUsage { get; set; }
        public string? Vin { get; set; }
        public int EngineVolume { get; set; }
        public int BatteryCapacity { get; set; }
        public byte BodyType { get; set; }
        public byte Places { get; set; }
        public byte Color { get; set; }
        public byte EngineType { get; set; }
        public int NetWeight { get; set; }
        public int GrossWeight { get; set; }
        public int VehicleKilowatts { get; set; }
        public byte SteeringWheel { get; set; }
        public DateTime FirstRegistrationDate { get; set; }
    }
}
