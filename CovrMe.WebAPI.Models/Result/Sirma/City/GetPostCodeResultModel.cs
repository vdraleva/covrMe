using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.City
{
    public class GetPostCodeResultModel
    {
        public GetPostCodeResultModel()
        {
            this.PostCodes = new List<SirmaBaseDataModel>();
        }
        public int Success { get; set; }

        [JsonProperty("result")]
        public List<SirmaBaseDataModel> PostCodes { get; set; }
    }
}
