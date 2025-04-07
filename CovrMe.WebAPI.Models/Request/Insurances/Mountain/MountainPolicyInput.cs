using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Insurances.Mountain
{
    public class MountainPolicyInput
    {
        public MountainPolicyInput()
        {
            this.Holder = new InsurerCustomerModel();
            this.Clients = new List<InsuredClientDataModel>();
        }
        public string? InsuranceCompany { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InsuranceSum { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsExtreme { get; set; }
        public string? LocalOrderNumber { get; set; }
        public string? MainUserId { get; set; }
        public InsurerCustomerModel? Holder { get; set; }
        public List<InsuredClientDataModel>? Clients { get; set; }
    }
}
