using CovrMe.Models.InsuranceCompanies.Result;
using CovrMe.Models.Insurances.Result.CivilInsurances;
using CovrMe.Models.Insurances.Result.HealthInsurance;
using CovrMe.Models.Insurances.Result.MountainInsurance;
using CovrMe.Models.Insurances.Result.MyThings;
using CovrMe.Models.Insurances.Result.TravelInsurance;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Vehicles.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result
{
    public class InsuranceModel
    {
        public InsuranceModel()
        {
            this.CivilInsurance = new CivilInsuranceModel(); 
            this.InsuranceCompany = new InsuranceCompanyModel();
            this.Vehicle = new VehicleResultModel();

            this.InsuredUsers = new List<InsuredUserModel>();

        }
        public string? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public DateTime CurrentEndDate { get; set; }
        public decimal Price { get; set; }
        public byte Type { get; set; }
        public int InstallmentsNumber { get; set; }
        public int InstallmentToPay { get; set; }
        public string? PolicyNo { get; set; }
        public InsuranceCompanyModel? InsuranceCompany { get; set; }

        public VehicleResultModel? Vehicle { get; set; }
        public CivilInsuranceModel? CivilInsurance { get; set; }
        public HealthInsuranceModel? HealthInsurance { get; set; }
        public MountainInsuranceModel? MountainInsurance { get; set; }
        public TravelInsuranceModel? TravelInsurance { get; set; }
        public MyThingsInsuranceModel? MyThingsInsurance { get; set; }
        public List<InsuredUserModel> InsuredUsers { get; set; }
    }
}
