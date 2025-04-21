using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsuranceSearchModel
    {
        public CivilInsuranceSearchModel()
        {
            this.Installments = new ObservableCollection<InstallmentModel>();
        }
        public ObservableCollection<InstallmentModel>? Installments { get; set; }
        public string? InsuranceCompanyName { get; set; }
        public string? LogoSrc { get; set; }
        public decimal Premium { get; set; }

        //formatted
        public string? PremiumFormatted { get; set; }

    }
}
