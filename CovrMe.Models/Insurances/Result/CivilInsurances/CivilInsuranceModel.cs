using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsuranceModel
    {
        public string? Id { get; set; }

        public decimal FirstInstallmentPrice { get; set; }

        public decimal SecondInstallmentPrice { get; set; }

        public decimal ThirdInstallmentPrice { get; set; }

        public decimal FourthInstallmentPrice { get; set; }

        public string? GreenCardNo { get; set; }

        public string? StickerNo { get; set; }

        public string? InsuranceId { get; set; }

        public DateTime? SecondInstallmentDate { get; set; }

        public DateTime? ThirdInstallmentDate { get; set; }

        public DateTime? FourthInstallmentDate { get; set; }

    }
}
