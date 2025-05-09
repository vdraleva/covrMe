﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.CivilInsurance.Request
{
    public class CivilInsuranceSearchInput
    {
        public int OwnerExperience { get; set; }

        public string? OwnerUiNumber { get; set; }
        public string? OwnerVatNumber { get; set; }

        public string? VehiclePlateNumber { get; set; }

        public string? VehicleUsage { get; set; }

        public string? RegistrationCertificateNumber { get; set; }

        public string? OwnerPostalCode { get; set; }

        public int VehicleType { get; set; }

        public string? OwnerCity { get; set; }

        public string? OwnerRegion { get; set; }

        public string? OwnerMunicipality { get; set; }

        public string OwnerCountryCode { get; set; }

        public string? OwnerName { get; set; }

        public string? OwnerAddress { get; set; }

        public int Installments { get; set; }

        public string OwnerBirthdate { get; set; }
        public string VehicleFirstReg { get; set; }

        public string? VehicleBrand { get; set; }

        public string? VehicleModel { get; set; }

        //usual

        public int UsualExperience { get; set; }

        public string? UsualUiNumber { get; set; }

        public string? UsualPostalCode { get; set; }

        public string? UsualRegion { get; set; }

        public string? UsualMunicipality { get; set; }

        public string? UsualName { get; set; }

        public string? UsualAddress { get; set; }

        public string? UsualBirthdate { get; set; }
        public string? UsualCountryCode { get; set; }
        public string? UsualCity { get; set; }
    }
}
