using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class MyThingsModel
    {
        public MyThingsModel()
        {
            this.Questionnaire = new QuestionnaireModel();
        }
        public int PropertyTypeId { get; set; }
        public int ObjectTypeId { get; set; }
        public decimal InsuranceSum { get; set; }
        public QuestionnaireModel? Questionnaire { get; set; }
    }
}
