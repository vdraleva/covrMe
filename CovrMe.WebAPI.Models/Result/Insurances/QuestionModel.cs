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
    public class QuestionModel : IMapFrom<Question>, IHaveCustomMappings
    {
        public string? Id { get; set; }
        public int QuestionId { get; set; }
        public string? Answer { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Question, QuestionModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));

        }
    }
}
