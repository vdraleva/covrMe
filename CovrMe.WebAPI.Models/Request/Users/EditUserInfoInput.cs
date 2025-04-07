using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Users
{
    public class EditUserInfoInput
    {
        public string? UserId { get; set; }
        public string? ParentUserId { get; set; }
        public string? FirstName { get; set; }
        public string? SurName { get; set; }
        public string? LastName { get; set; }
        public string? LatinFirstName { get; set; }
        public string? LatinSurName { get; set; }
        public string? LatinLastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? UiNumber { get; set; }
        public string? VatNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? CityId { get; set; }
        public string? CountryId { get; set; }
        public string? RegionId { get; set; }
        public string? MuniciplalityId { get; set; }
        public string? PostCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? DrivingExperience { get; set; }
        public string? LatinAddress { get; set; }
        public string? LatinCompanyName { get; set; }
    }
}
