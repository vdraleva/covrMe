using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.MyThings
{
    public class MyThingsInsuranceModel
    {
        public string? Id { get; set; }

        public byte PropertyType { get; set; }

        public byte ObjectType { get; set; }

        public decimal InsuranceSum { get; set; }
        public string? InsuranceId { get; set; }
        public string? Brand { get; set; } = null!;

        public string? Model { get; set; } = null!;
    }
}
