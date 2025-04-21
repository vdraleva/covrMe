using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.HealthInsurance
{
    public class HealthPolicyInput
    {
        public HealthPolicyInput()
        {
            this.Clients = new List<InsuredClientDataModel>();
        }
        public string? InsuranceCompany { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PacketId { get; set; }
        public int InstallmentCount { get; set; }
        public bool IsFamily { get; set; }
        public string? LocalOrderNumber { get; set; }
        public string? MainUserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public InsurerCustomerModel? Holder { get; set; }
        public QuestionnaireModel Questionnaire { get; set; }
        public List<InsuredClientDataModel>? Clients { get; set; }
    }
}
