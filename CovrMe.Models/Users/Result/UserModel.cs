using CovrMe.Models.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result
{
    public class UserModel : BaseResultModel
    {
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

        //[JsonConverter(typeof(CustomDateTimeConverter), "yyyy-MM-ddTHH:mm:ss.fffK", "en-US")]
        public DateTime? BirthDate { get; set; }
        public string? LatinAddress { get; set; }
        public string? LatinCompanyName { get; set; }
    }
}
