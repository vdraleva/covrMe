using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Insurances
{
    public class InsurerCustomerModel
    {
        public InsurerCustomerModel()
        {
            this.InsuredType = 16;
        }

        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Uin { get; set; }
        public string? Vat { get; set; }
        public string? CountryOriginal { get; set; }
        public string? Country { get; set; }
        public string? Location { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int InsuredType { get; set; }
    }
}
