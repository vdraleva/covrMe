using CovrMe.Models.Insurances.Result.TravelInsurance;
using Newtonsoft.Json;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class TravelPolicyPayload
    {
        public TravelPolicyPayload()
        {
            this.Result = new TravelPolicyResultModel();
        }

        [JsonProperty("travelPolicy")]
        public TravelPolicyResultModel? Result { get; set; }
    }
}
