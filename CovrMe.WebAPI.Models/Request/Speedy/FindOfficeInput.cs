using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy
{
    public class FindOfficeInput
    {
        public string? City { get; set; }
        public string? Neighborhood { get; set; }
    }
}
