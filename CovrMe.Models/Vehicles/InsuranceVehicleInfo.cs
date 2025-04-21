using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles
{
    public class InsuranceVehicleInfo
    {
        public string? VehicleId { get; set; }
        public string? PlateNumber { get; set; }
        public string? RegistrationCertificateNumber { get; set; }
        public string? VehicleBrand { get; set; }
        public string? VehicleModel { get; set; }
        public int VehicleBrandId { get; set; }
        public int VehicleModelId { get; set; }
        public string? FirstRegistrationDate { get; set; }
        public DateTime FirstRegistrationDateAsDateTime { get; set; }
        public string? VehicleType { get; set; }
        public int VehicleTypeId { get; set; }
        public string? VehicleUsage { get; set; }
        public int VehicleUsageId { get; set; }
        public string? Vin { get; set; }
        public int EngineVolume { get; set; }
        public byte BodyType { get; set; }
        public string? BodyTypeText { get; set; }
        public byte Places { get; set; }
        public byte Color { get; set; }
        public string? ColorText { get; set; }
        public int EngineType { get; set; }
        public string? EngineTypeText { get; set; }
        public int NetWeight { get; set; }
        public int GrossWeight { get; set; }
        public int VehicleKilowatts { get; set; }
        public int BatteryCapacity { get; set; }
        public byte SteeringWheel { get; set; }
    }
}
