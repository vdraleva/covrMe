using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Insurances.CivilInsurance
{
    public class CivilInsuranceLongPolicyInput
    {
        public string? InsuranceCompany { get; set; }
        public string? Email { get; set; }

        //vehicle

        public string? VehicleFirstReg { get; set; }

        public string? VehicleBrand { get; set; }

        public string? VehicleModel { get; set; }

        public int VehicleModelId { get; set; }
        public string? VehiclePlateNumber { get; set; }

        public string? VehicleUsage { get; set; }

        public int VehicleType { get; set; }

        public string? RegistrationCertificateNumber { get; set; }

        public string? Vin { get; set; }
        public int VehicleEngineVolume { get; set; }
        public string? VehicleBodyType { get; set; }
        public int VehiclePlaces { get; set; }
        public string? VehicleColor { get; set; }
        public string? VehicleEngineType { get; set; }
        public int VehicleSteeringWheel { get; set; }
        public int VehicleKilowatts { get; set; }

        //owner

        public int OwnerGuilt { get; set; }
        public int OwnerExperience { get; set; }
        public string? OwnerUiNumber { get; set; }
        public string? OwnerVatNumber { get; set; }
        public string? OwnerPostalCode { get; set; }

        public string? OwnerCity { get; set; }

        public string? OwnerRegion { get; set; }

        public string? OwnerMunicipality { get; set; }

        public string? OwnerCountryCode { get; set; }

        public string? OwnerName { get; set; }

        public string? OwnerAddress { get; set; }

        public int Installments { get; set; }

        public string? OwnerBirthdate { get; set; }
        public string? UserId { get; set; }
        public string? MainUserId { get; set; }

        //usual

        public int UsualGuilt { get; set; }
        public int UsualExperience { get; set; }
        public string? UsualUiNumber { get; set; }
        public string? UsualPostalCode { get; set; }
        public string? UsualRegion { get; set; }
        public string? UsualMunicipality { get; set; }
        public string? UsualName { get; set; }
        public string? UsualAddress { get; set; }
        public string? UsualBirthdate { get; set; }
        public string? UsualCountryCode { get; set; }
        public string? UsualUserId { get; set; }
        public string? UsualCity { get; set; }

        //insurance
        public DateTime? StartDate { get; set; }
        public string? GreenCardId { get; set; }
        public string? StickerId { get; set; }

        //payment
        public string? LocalOrderNumber { get; set; }
        public string? Phone { get; set; }
    }
}
