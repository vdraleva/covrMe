using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Result
{
    public class InsuredUserModel
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SurName { get; set; }
        public bool IsInsurer { get; set; }
        public bool IsMainUser { get; set; }
        public bool IsUsualDriver { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsInsured { get; set; }

        public string? UiNumber { get; set; }
        public string? VatNumber { get; set; }
        public string? CompanyName { get; set; }
    }
}
