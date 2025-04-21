using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.TravelInsurance
{
    public class TravelPolicyInput
    {
        public TravelPolicyInput()
        {
            this.Holder = new InsurerCustomerModel();
            this.Clients = new List<InsuredClientDataModel>();
        }
        public string? InsuranceCompany { get; set; }
        public byte PolicyType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte TripPurpose { get; set; }
        public byte Territory { get; set; }
        public decimal Limit { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? LocalOrderNumber { get; set; }
        public string? MainUserId { get; set; }
        public InsurerCustomerModel? Holder { get; set; }
        public List<InsuredClientDataModel>? Clients { get; set; }
    }
}
