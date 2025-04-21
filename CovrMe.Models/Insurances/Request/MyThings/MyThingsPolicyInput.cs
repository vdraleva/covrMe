using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.MyThings
{
    public class MyThingsPolicyInput
    {
        public MyThingsPolicyInput()
        {
            this.Questionnaire = new QuestionnaireModel();
        }
        public string? InsuranceCompany { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? LocalOrderNumber { get; set; }
        public string? MainUserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public InsurerCustomerModel? Holder { get; set; }
        public int PropertyTypeId { get; set; }
        public int ObjectTypeId { get; set; }
        public decimal InsuranceSum { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public QuestionnaireModel? Questionnaire { get; set; }
    }
}
