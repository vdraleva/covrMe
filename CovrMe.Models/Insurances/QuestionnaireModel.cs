using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class QuestionnaireModel
    {
        public QuestionnaireModel()
        {
            this.Questionnaire = new List<QuestionModel>();
        }
        public List<QuestionModel>? Questionnaire { get; set; }
    }
}
