using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.MyThings
{
    public class MyThingsCalculationInput
    {
        public MyThingsCalculationInput()
        {
            this.Questionnaire = new QuestionnaireModel();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PropertyTypeId { get; set; }
        public int ObjectTypeId { get; set; }
        public decimal InsuranceSum { get; set; }
        public QuestionnaireModel? Questionnaire { get; set; }
    }
}
