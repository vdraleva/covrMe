using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class InsuredClientDataModel
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Uin { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
