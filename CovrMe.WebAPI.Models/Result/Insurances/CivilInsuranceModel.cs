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
    public class CivilInsuranceModel : IMapFrom<CivilInsurance>, IHaveCustomMappings
    {
        public string? Id { get; set; }       
        public decimal FirstInstallmentPrice { get; set; }
        public decimal FirstInstallmentTax { get; set; }
        public decimal SecondInstallmentPrice { get; set; }
        public decimal SecondInstallmentTax { get; set; }
        public decimal ThirdInstallmentPrice { get; set; }
        public decimal ThirdInstallmentTax { get; set; }
        public decimal FourthInstallmentPrice { get; set; }
        public decimal FourthInstallmentTax { get; set; }        
        public string? GreenCardNo { get; set; }
        public string? StickerNo { get; set; }
        public string? InsuranceId { get; set; }
        public DateTime? SecondInstallmentDate { get; set; }
        public DateTime? ThirdInstallmentDate { get; set; }
        public DateTime? FourthInstallmentDate { get; set; }        
        public string? GreenCardPdfUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CivilInsurance, CivilInsuranceModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));

            configuration.CreateMap<MountainInsurance, MountainInsuranceModel>().ForMember(
                m => m.InsuranceId,
                opt => opt.MapFrom(x => x.InsuranceId.ToString()));
        }

    }
}
