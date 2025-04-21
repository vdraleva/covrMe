using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class InsuredUsersModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Uin { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool Save { get; set; }
        public bool IsFilled { get; set; }
        public int Index { get; set; }
    }
}
