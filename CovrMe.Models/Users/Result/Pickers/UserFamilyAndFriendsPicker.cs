using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result.Pickers
{
    public class UserFamilyAndFriendsPicker
    {
        public string? Id { get; set; }

        public int DrivingExperience { get; set; }

        public string? FirstName { get; set; } = null!;
                     
        public string? SurName { get; set; } = null!;
                     
        public string? LastName { get; set; } = null!;

        public string? LatinFirstName { get; set; }

        public string? LatinSurName { get; set; }

        public string? LatinLastName { get; set; }

        public string? UiNumber { get; set; }

        public string? VatNumber { get; set; }

        public string? Region { get; set; }

        public string? Nationality { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? Address { get; set; }
        public string? LatinAddress { get; set; }

        public string? Municipality { get; set; }

        public string? AspNetUserId { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? ParentUserId { get; set; }

        public string? Email { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Names { get; set; }
        public  string? CompanyName { get; set; }
        public string? LatinCompanyName { get; set; }
        public int GuiltTypeId { get; set; }
    }
}
