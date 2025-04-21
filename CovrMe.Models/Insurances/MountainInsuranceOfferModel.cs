using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class MountainInsuranceOfferModel
    {
        public int InsuranceSum { get; set; }
        public string? InsuranceSumFormatted { get; set; }
        public bool IsExtreme { get; set; }
        public byte NumberUnder18 { get; set; }
        public byte NumberOver18 { get; set; }
        public string? Username { get; set; }
    }
}
