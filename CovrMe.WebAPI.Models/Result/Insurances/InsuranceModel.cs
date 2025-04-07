using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Models.Result.InsuranceCompanies;
using CovrMe.WebAPI.Models.Result.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class InsuranceModel
    {
        public InsuranceModel()
        {
            this.CivilInsurance = new CivilInsuranceModel();
            this.HealthInsurance = new HealthInsuranceModel();
            this.MountainInsurance = new MountainInsuranceModel();
            this.TravelInsurance = new TravelInsuranceModel();
            this.InsuranceCompany = new InsuranceCompanyModel();
            this.Vehicle = new VehicleResultModel();
            this.MyThingsInsurance = new MyThingsInsuranceModel();

            this.InsuredUsers = new List<InsuredUserModel>();

        }
        public string? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public DateTime CurrentEndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public byte Status { get; set; }
        public decimal Price { get; set; }
        public string? CurrencyCode { get; set; }
        public byte Type { get; set; }
        public string? PdfUrl { get; set; }
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
