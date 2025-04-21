using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Locations.Result
{
    public class CityModel
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? PostalCode { get; set; }
        public Guid MunicipalityId { get; set; }
    }
}
