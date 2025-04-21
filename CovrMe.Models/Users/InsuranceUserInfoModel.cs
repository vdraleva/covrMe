using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users
{
    public class InsuranceUserInfoModel
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? SurName { get; set; }
        public string? LastName { get; set; }
        public string? Uin { get; set; }
        public string? VatNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string? RegionCode { get; set; }
        public string? MunicipalityCode { get; set; }
        public string? PostalCode { get; set; }
        public string? CityCode { get; set; }
        public string? CountryCode { get; set; }
        public int DrivingExperiance { get; set; }
        public string? RegionName { get; set; }
        public string? MunicipalityName { get; set; }
        public string? CityName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? BirthDateString { get; set; }
        public int Age { get; set; }
        public bool IsInsured { get; set; }
        public int GuiltTypeId { get; set; }
    }
}
