using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models
{
    public class CountryTelCode
    {
        public string PhoneCode { get; set; }
        public string CountryShortCode { get; set; }

        public CountryTelCode(string phoneCode, string countryShortCode)
        {
            PhoneCode = phoneCode;
            CountryShortCode = countryShortCode;
        }
    }
}
