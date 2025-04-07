using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Users
{
    public class RegisterUserInput
    {
        public string? FirstName { get; set; }
        public string? SurName { get; set; }
        public string? LastName { get; set; }
        public string? UiNumber { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? VatNumber { get; set; }
        public string? CompanyName { get; set; }
    }
}
