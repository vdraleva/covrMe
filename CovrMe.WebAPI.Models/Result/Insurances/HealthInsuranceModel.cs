using AutoMapper;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Shared.Mapping;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class HealthInsuranceModel : IMapFrom<HealthInsurance>, IHaveCustomMappings
    {
        public string? Id { get; set; }

        public byte PackageType { get; set; }

        public decimal InstallmentPrice { get; set; }

        public string? InsuranceId { get; set; }
        public bool IsFamily { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<HealthInsurance, HealthInsuranceModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));

            configuration.CreateMap<HealthInsurance, HealthInsuranceModel>().ForMember(
                m => m.InsuranceId,
                opt => opt.MapFrom(x => x.InsuranceId.ToString()));
        }
    }
}
