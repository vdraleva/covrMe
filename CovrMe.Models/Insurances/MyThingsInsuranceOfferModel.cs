using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class MyThingsInsuranceOfferModel
    {
        public MyThingsInsuranceOfferModel()
        {
            this.Questionnaire = new QuestionnaireModel();
        }
        public int PropertyTypeId { get; set; }
        public int ObjectTypeId { get; set; }
        public decimal InsuranceSum { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public QuestionnaireModel? Questionnaire { get; set; }
    }
}
