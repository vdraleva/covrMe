using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Pickers
{
    public class UserVehiclesPickerModel
    {
        public string? VehicleId { get; set; }
        public string? VehicleBrandAndModel { get; set; }
        public string? RegistrationCertificateNumber { get; set; }
        public string? VehiclePlateNumber { get; set; }
        public int VehicleModelId { get; set; }
        public int VehicleTypeId { get; set; }
        public int VehicleUsageId { get; set; }
        public int VehicleBrandId { get; set; }
        public DateTime? FirstRegistrationDate { get; set; }
        public string? Vin { get; set; }
        public int EngineVolume { get; set; }
        public int BodyTypeId { get; set; }
        public int Places { get; set; }
        public int ColorId { get; set; }
        public int EngineTypeId { get; set; }
        public int NetWeight { get; set; }
        public int GrossWeight { get; set; }
        public int VehicleKilowatts { get; set; }
        public int BatteryCapacity { get; set; }
        public int SteeringWheelId { get; set; }
    }
}
