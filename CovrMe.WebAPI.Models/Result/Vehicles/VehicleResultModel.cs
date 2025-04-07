using AutoMapper;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CovrMe.WebAPI.Models.Result.Vehicles
{
    public class VehicleResultModel : BaseResultModel,IMapFrom<Vehicle>, IHaveCustomMappings
    {
        public string? Id { get; set; }
        public string? RegistrationCertificateNumber { get; set; }
        public string? PlateNumber { get; set; }
        public string? UserId { get; set; }
        public int ModelId { get; set; }
        public int BrandId { get; set; }
        public DateTime? FirstRegistrationDate { get; set; }
        public int VehicleTypeId { get; set; }
        public int VehicleUsageId { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
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
        public bool? IsDeleted { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Vehicle, VehicleResultModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));

            configuration.CreateMap<Vehicle, VehicleResultModel>().ForMember(
                m => m.UserId,
                opt => opt.MapFrom(x => x.UserId.ToString()));

        }
    }
}
