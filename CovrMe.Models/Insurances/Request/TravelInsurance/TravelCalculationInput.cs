using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.TravelInsurance
{
    public class TravelCalculationInput
    {
        public byte PolicyType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte TripPurpose { get; set; }
        public byte Territory { get; set; }
        public decimal Limit { get; set; }
        public byte NumberUnder18 { get; set; }
        public byte Number18To65 { get; set; }
        public byte Number66To70 { get; set; }
        public byte Number71To75 { get; set; }
        public byte Number76To80 { get; set; }
        public byte NumberOver80 { get; set; }
    }
}
