using AutoMapper;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Payment
{
    public class PaymentInfoModel : IMapFrom<PaymentInformation>, IHaveCustomMappings
    {
        public string? Id { get; set; }

        public string? LocalOrderNumber { get; set; }

        public string? DskOrderNumber { get; set; } = null!;
        public int Status { get; set; }

        public string? Operation { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<PaymentInformation, PaymentInfoModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));
        }
    }
}
