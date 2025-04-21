using CovrMe.Models.Insurances.Result.CivilInsurances;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class UpdateCivilInsuranceDocumentStatusPayload
    {
        [JsonProperty("updateCivilInsuranceDocumentStatus")]
        public UpdateCivilInsuranceDocumentStatusResultModel Info { get; set; }
    }
}
