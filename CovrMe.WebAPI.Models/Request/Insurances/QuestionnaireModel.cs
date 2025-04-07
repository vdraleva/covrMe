using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CovrMe.WebAPI.Models.Result.Insurances;

namespace CovrMe.WebAPI.Models.Request.Insurances
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
