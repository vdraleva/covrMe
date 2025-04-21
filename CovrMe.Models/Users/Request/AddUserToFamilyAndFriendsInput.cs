using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Request
{
    public class AddUserToFamilyAndFriendsInput
    {
        public string? FirstName { get; set; }
        public string? SurName { get; set; }
        public string? LastName { get; set; }
        public string? LatinFirstName { get; set; }
        public string? LatinSurName { get; set; }
        public string? LatinLastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? UiNumber { get; set; }
        public string? Address { get; set; }
        public string? VatNumber { get; set; }
        public string? CompanyName { get; set; }
        public int? DrivingExperience { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? UserId { get; set; }
        public string? CityId { get; set; }
        public string? CountryId { get; set; }
        public string? RegionId { get; set; }
        public string? MuniciplalityId { get; set; }
        public string? PostCode { get; set; }
        public string? LatinAddress { get; set; }
        public string? LatinCompanyName { get; set; }
    }
}
