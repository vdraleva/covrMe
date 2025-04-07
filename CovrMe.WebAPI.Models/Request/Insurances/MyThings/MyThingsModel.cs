using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Insurances.MyThings
{
    public class MyThingsModel
    {
        public int PropertyTypeId { get; set; }
        public int ObjectTypeId { get; set; }
        public decimal InsuranceSum { get; set; }
        public QuestionnaireModel? Questionnaire { get; set; }
    }
}
