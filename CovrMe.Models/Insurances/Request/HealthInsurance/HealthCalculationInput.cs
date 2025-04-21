using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.HealthInsurance
{
    public class HealthCalculationInput
    {
        public HealthCalculationInput()
        {
            this.InsuredBirthDate = new List<DateTime>();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PacketId { get; set; }
        public int InstallmentCount { get; set; }
        public bool IsFamily { get; set; }
        public List<DateTime>? InsuredBirthDate { get; set; }
    }
}
