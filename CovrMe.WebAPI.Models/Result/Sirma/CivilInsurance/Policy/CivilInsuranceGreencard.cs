using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy
{
    public class CivilInsuranceGreencard
    {
        [JsonProperty("upn")]
        public string? GreenCardNumber { get; set; }

        [JsonProperty("pdf_url")]
        public string? PdfUrl { get; set; }
    }
}
