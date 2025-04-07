using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma
{
    public class SirmaAuthenticationResultModel
    {
        public int Success { get; set; }
        public string Token { get; set; }
        public SirmaAuthenticationInfoResultModel Info { get; set; }

    }
}
