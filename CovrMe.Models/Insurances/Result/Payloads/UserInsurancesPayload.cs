using Newtonsoft.Json;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class UserInsurancesPayload
    {
        [JsonProperty("userInsurances")]
        public UserInsurancesResultModel? UserInsurances { get; set; }
    }
}
