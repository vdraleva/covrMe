using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class ExpiringInsuranceModel
    {
        public string TypeInsurance { get; set; }
        public string ExpireDate { get; set; }
        public string CarNumber { get; set; }
        public string BrandAndModel { get; set; }
        public string InsuranceCompany { get; set; }
    }
}
