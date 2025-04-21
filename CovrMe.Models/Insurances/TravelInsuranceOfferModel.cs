using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class TravelInsuranceOfferModel
    {
        public byte PolicyType { get; set; }
        public byte TripPurpose { get; set; }
        public byte Territory { get; set; }
        public decimal Limit { get; set; }
        public string? LimitFormatted { get; set; }
        public string? Username { get; set; }
    }
}
