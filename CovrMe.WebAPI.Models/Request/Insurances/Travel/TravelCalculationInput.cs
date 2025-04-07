using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Insurances.Travel
{
    public class TravelCalculationInput
    {
        public byte PolicyType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte TripPurpose { get; set; }
        public byte Territory { get; set; }
        public decimal Limit { get; set; }
        public byte NumberUnder18 { get; set; } //NumberAge1
        public byte Number18To65 { get; set; } //NumberAge2
        public byte Number66To70 { get; set; } //NumberAge3
        public byte Number71To75 { get; set; } //NumberAge4
        public byte Number76To80 { get; set; } //NumberAge5
        public byte NumberOver80 { get; set; } //NumberAge6
    }
}
