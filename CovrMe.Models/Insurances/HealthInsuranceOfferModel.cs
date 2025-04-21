using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class HealthInsuranceOfferModel
    {
        public HealthInsuranceOfferModel()
        {
            this.InsuredBirthDate = new List<DateTime>();
        }
        public int PacketId { get; set; }
        public bool IsFamily { get; set; }
        public QuestionnaireModel Questionnaire { get; set; }
        public List<DateTime>? InsuredBirthDate { get; set; }
    }
}
