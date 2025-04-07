using AutoMapper;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.InsuranceCompanies
{
    public class InsuranceCompanyModel : IMapFrom<InsuranceCompany>, IHaveCustomMappings
    {
        public string? Id { get; set; }

        public string CompanyName { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<InsuranceCompany, InsuranceCompanyModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));

        }
    }
}
