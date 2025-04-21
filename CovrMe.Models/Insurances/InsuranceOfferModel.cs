using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CovrMe.Models.Insurances.Result.TravelInsurance;

namespace CovrMe.Models.Insurances
{
    public class InsuranceOfferModel : BaseInsuranceOfferModel
    {
        public InsuranceOfferModel()
        {
            this.TravelInsuranceInfo = new TravelInsuranceOfferModel();
            this.MountainInsuranceInfo = new MountainInsuranceOfferModel();
            this.HealthInsuranceInfo = new HealthInsuranceOfferModel();
            this.MyThingsInsuranceInfo = new MyThingsInsuranceOfferModel();
        }

        public string? StartDateFormatted { get; set; }
        public string? EndDateFormatted { get; set; }
        public string? Email { get; set; }
        public string? PriceFormatted { get; set; }
        public string? TaxFormatted { get; set; }
        public string? PriceWithoutTaxFormatted { get; set; }

        public TravelInsuranceOfferModel? TravelInsuranceInfo { get; set; }
        public MountainInsuranceOfferModel? MountainInsuranceInfo { get; set; }
        public HealthInsuranceOfferModel? HealthInsuranceInfo { get; set; }
        public MyThingsInsuranceOfferModel? MyThingsInsuranceInfo { get; set; }
    }
}
