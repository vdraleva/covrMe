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
    public class MyThingsInsuranceModel : IMapFrom<MyThingsInsurance>, IHaveCustomMappings
    {
        public string? Id { get; set; }

        public byte PropertyType { get; set; }

        public byte ObjectType { get; set; }

        public decimal InsuranceSum { get; set; }
        public string? InsuranceId { get; set; }
        public string? Brand { get; set; } = null!;

        public string? Model { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MyThingsInsurance, MyThingsInsuranceModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));

            configuration.CreateMap<MyThingsInsurance, MyThingsInsuranceModel>().ForMember(
                m => m.InsuranceId,
                opt => opt.MapFrom(x => x.InsuranceId.ToString()));
        }
    }
}
