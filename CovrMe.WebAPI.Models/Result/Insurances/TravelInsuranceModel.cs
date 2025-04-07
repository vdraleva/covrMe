using AutoMapper;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class TravelInsuranceModel : IMapFrom<TravelInsurance>, IHaveCustomMappings
    {
        public string? Id { get; set; }

        public byte TerritorialValidity { get; set; }

        public byte TravelPurpose { get; set; }

        public decimal CompensationAmount { get; set; }

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
