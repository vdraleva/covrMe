using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Sirma
{
    public class CivilInsurancePolicyRequestModel
    {
        [JsonProperty("owner_experience")]
        public int OwnerExperience { get; set; }

        [JsonProperty("owner_ein")]
        public string? UiNumber { get; set; }

        [JsonProperty("reg_num")]
        public string? VehiclePlateNumber { get; set; }

        [JsonProperty("usage")]
        public string? VehicleUsage { get; set; }

        [JsonProperty("registration_certificate")]
        public string? RegistrationCertificateNumber { get; set; }

        [JsonProperty("owner_zip")]
        public string? PostalCode { get; set; }

        [JsonProperty("insurer")]
        public string? Insurer { get; set; }

        [JsonProperty("vehicle_type")]
        public int VehicleType { get; set; }

        [JsonProperty("owner_person_type")]
        public int OwnerPersonType { get; set; }

        [JsonProperty("office")]
        public string? Office { get; set; }

        [JsonProperty("owner_city")]
        public string? City { get; set; }

        [JsonProperty("owner_region")]
        public string? Region { get; set; }

        [JsonProperty("owner_municipality")]
        public string? Мunicipality { get; set; }

        [JsonProperty("owner_nationality")]
        public int Nationality { get; set; }

        [JsonProperty("owner_name")]
        public string? Name { get; set; }

        [JsonProperty("owner_address")]
        public string? Address { get; set; }

        [JsonProperty("installments")]
        public int Installments { get; set; }

        [JsonProperty("owner_dob")]
        public string? Birthdate { get; set; }

        [JsonProperty("first_reg")]
        public string? VehicleFirstReg { get; set; }

        [JsonProperty("mark")]
        public string? VehicleBrand { get; set; }

        [JsonProperty("model")]
        public string? VehicleModel { get; set; }

        [JsonProperty("has_usual")]
        public int HasUsualDriver { get; set; }

        [JsonProperty("usual_experience")]
        public int UsualExperience { get; set; }

        [JsonProperty("usual_ein")]
        public string? UiNumberUsual { get; set; }

        [JsonProperty("usual_zip")]
        public string? PostalCodeUsual { get; set; }

        [JsonProperty("usual_person_type")]
        public int UsualPersonType { get; set; }

        [JsonProperty("usual_region")]
        public string? RegionUsual { get; set; }

        [JsonProperty("usual_municipality")]
        public string? МunicipalityUsual { get; set; }

        [JsonProperty("usual_nationality")]
        public int NationalityUsual { get; set; }

        [JsonProperty("usual_name")]
        public string? NameUsual { get; set; }

        [JsonProperty("usual_address")]
        public string? AddressUsual { get; set; }

        [JsonProperty("usual_dob")]
        public string? BirthdateUsual { get; set; }

        [JsonProperty("owner_country_code")]
        public string? OwnerCountryCode { get; set; }

        [JsonProperty("usual_country_code")]
        public string? UsualCountryCode { get; set; }

        [JsonProperty("usual_city")]
        public string? UsualCity { get; set; }

        [JsonProperty("sticker")]
        public string? StickerNo { get; set; }

        [JsonProperty("greencard_number")]
        public string? GreencardNo { get; set; }

        [JsonProperty("start_date")]
        public string? PolicyStartDate { get; set; }

        [JsonProperty("policy_id")]
        public string? PolicyNo { get; set; }
    }
}
