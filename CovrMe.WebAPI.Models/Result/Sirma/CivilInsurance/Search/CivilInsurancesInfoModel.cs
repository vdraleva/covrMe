using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search
{
    public class CivilInsurancesInfoModel
    {
        [JsonProperty("dzi")]
        public DziCivilInsuranceModel? DziCivilInsurance { get; set; }

        [JsonProperty("euroins")]
        public EuroinsCivilInsuranceModel? EuroinsCivilInsurance { get; set; }

        [JsonProperty("generali")]
        public GeneraliCivilInsuranceModel? GeneraliCivilInsurance { get; set; }

        [JsonProperty("bulins")]
        public BulinsCivilInsuranceModel? BulinsCivilInsurance { get; set; }

        [JsonProperty("bulstrad")]
        public BulstradCivilInsuranceModel? BulstradCivilInsurance { get; set; }

        [JsonProperty("ozk")]
        public OzkCivilInsuranceModel? OzkCivilInsurance { get; set; }

        [JsonProperty("groupama")]
        public GroupamaCivilInsuranceModel? GroupamaCivilInsurance { get; set; }

        [JsonProperty("levins")]
        public LevinsCivilInsuranceModel? LevinsCivilInsurance { get; set; }
    }
}
