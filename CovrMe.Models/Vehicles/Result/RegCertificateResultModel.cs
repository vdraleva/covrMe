using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result
{
    public class RegCertificateResultModel : BaseResultModel
    {
        public string? RegistrationCertificateNumber { get; set; }
        public string? PlateNumber { get; set; }
        public DateTime? FirstRegistrationDate { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
        public string? VehicleType { get; set; }

        // Expanded search
        public string? Vin { get; set; }
        public string? EngineVolume { get; set; }
        public string? BodyType { get; set; }
        public string? Color { get; set; }
        public string? EngineType { get; set; }
        public string? NetWeight { get; set; }
        public string? GrossWeight { get; set; }
        public string? VehicleKilowatts { get; set; }
        public string? SteeringWheel { get; set; }
        public string? Places { get; set; }

        // Owner
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? LastName { get; set; }

        public string? Uin { get; set; }
        public string? Region { get; set; }
        public string? Municipality { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
