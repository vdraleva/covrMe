using AutoMapper;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Models.Result.Vehicles;
using CovrMe.WebAPI.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Users
{
    public class UserModel : BaseResultModel, IMapFrom<User>, IHaveCustomMappings
    {
        public UserModel()
        {
            this.Roles = new List<string>();
        }
        public string? Id { get; set; }
        public int? DrivingExperience { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? SurName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? LatinFirstName { get; set; }
        public string? LatinSurName { get; set; }
        public string? LatinLastName { get; set; }
        public string? UiNumber { get; set; }
        public string? VatNumber { get; set; }
        public string? Address { get; set; }
        public string? RegionId { get; set; }
        public string? CityId { get; set; }
        public string? CountryId { get; set; }
        public string? MunicipalityId { get; set; }
        public string? PostCode { get; set; }
        public string? AspNetUserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public Guid? ParentUserId { get; set; }
        public string? Email { get; set; }
        public bool IsDeleted { get; set; }
        public string? LatinAddress { get; set; }
        public string? LatinCompanyName { get; set; }
        public DateTime? BirthDate { get; set; }
        public ICollection<string>? Roles { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<User, UserModel>().ForMember(
                m => m.Id,
                opt => opt.MapFrom(x => x.Id.ToString()));
        }
    }
}
