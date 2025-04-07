using AutoMapper;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Shared.Mapping;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class MountainInsuranceModel : IMapFrom<MountainInsurance>, IHaveCustomMappings
    {
        public string? Id { get; set; }

        public decimal CompensationAmount { get; set; }

        public bool IsExtreme { get; set; }

        public string? InsuranceId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MountainInsurance, MountainInsuranceModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));

            configuration.CreateMap<MountainInsurance, MountainInsuranceModel>().ForMember(
                m => m.InsuranceId,
                opt => opt.MapFrom(x => x.InsuranceId.ToString()));
        }
    }
}
