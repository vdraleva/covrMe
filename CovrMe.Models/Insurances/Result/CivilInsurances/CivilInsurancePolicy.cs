using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsurancePolicy
    {
        public string? PolicyNumber { get; set; }

        public string? EndDateString { get; set; }
        public DateTime IssueDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? PdfUrl { get; set; }
    }
}
