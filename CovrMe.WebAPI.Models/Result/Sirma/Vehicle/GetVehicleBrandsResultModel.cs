using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.Vehicle
{
    public class GetVehicleBrandsResultModel
    {
        public GetVehicleBrandsResultModel()
        {
            this.Brands = new List<SirmaBaseDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<SirmaBaseDataModel> Brands { get; set; }
    }
}
