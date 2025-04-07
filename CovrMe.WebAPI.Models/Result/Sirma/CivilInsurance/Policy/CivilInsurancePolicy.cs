using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy
{
    public class CivilInsurancePolicy
    {
        [JsonProperty("upn")]
        public string? PolicyNumber { get; set; }

        [JsonProperty("end_date")]
        public string? EndDate { get; set; }

        [JsonProperty("pdf_url")]
        public string? PdfUrl { get; set; }
    }
}
