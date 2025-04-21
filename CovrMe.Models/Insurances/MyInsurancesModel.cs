using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class MyInsurancesModel
    {
        public string? InsuranceId { get; set; }
        public string? InsuredUserId { get; set; }
        public int InstallmentToPay { get; set; }
        public string? InsuranceType { get; set; }
        public string? ExpireDate { get; set; }
        public string? Name { get; set; }
        public string? InsuranceCompany { get; set; }
        public string? InsuranceCompanyCode { get; set; }
        public string? BtnSource { get; set; }
        public string? StatusImgSource { get; set; }
        public bool? ShowBtn { get; set; }
        public bool? IsExpiring { get; set; }
        public bool? IsCivil { get; set; }
        public string? UserId { get; set; }
        public string? PolicyNo { get; set; }
        public string? GreenCardNo { get; set; }
    }
}
