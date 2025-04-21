using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Shared.Constants
{
    public class Queries
    {
        #region Users
        public const string GetUserFamilyAndFriends = "query($userId: String!){\r\n    getUsersFamilyAndFriends(userId: $userId){\r\n        nodes{\r\n            email,\r\n        id,\r\n        drivingExperience,\r\n        phoneNumber,\r\n        firstName,\r\n        surName,\r\n        lastName,\r\n        latinFirstName,\r\n        latinSurName,\r\n        latinLastName,\r\n        uiNumber,\r\n        vatNumber,\r\n        address,\r\n        birthDate,\r\n        address,\r\n        regionId,\r\n        municipalityId,\r\n        cityId,\r\n        countryId,\r\n        postCode,\r\n        latinAddress,\r\n        latinCompanyName,\r\n        companyName\r\n        }\r\n    }\r\n}";
        public const string GetUserById = "query($userId: String!){\r\n    user(userId: $userId){\r\n        nodes{\r\n         email,\r\n        id,\r\n        drivingExperience,\r\n        phoneNumber,\r\n        firstName,\r\n        surName,\r\n        lastName,\r\n        latinFirstName,\r\n        latinSurName,\r\n        latinLastName,\r\n        uiNumber,\r\n        vatNumber,\r\n        address,\r\n        birthDate,\r\n        address,\r\n        regionId,\r\n        municipalityId,\r\n        cityId,\r\n        countryId,\r\n        postCode\r\n        latinAddress,\r\n        latinCompanyName,\r\n        companyName\r\n        }\r\n    }\r\n}";
        public const string GetUserVatUin = "query($userId: String!){\r\n    user(userId: $userId){\r\n        nodes{      \r\n            uiNumber,\r\n            vatNumber        \r\n        }\r\n    }\r\n}";
        #endregion

        #region Locations
        public const string GetCities = "query($municipalityId:String!){\r\n    cities(municipalityId: $municipalityId){\r\n        nodes{\r\n          code,\r\n          name,\r\n          id,\r\n          postalCode\r\n        }\r\n    }\r\n}";
        public const string GetCountries = "query{\r\n    countries{\r\n        nodes{\r\n          code,\r\n          name,\r\n          id\r\n        }\r\n    }\r\n}";
        public const string GetMunicipality = "query($regionId:String!){\r\n    municipality(regionId: $regionId){\r\n        nodes{\r\n          code,\r\n          name,\r\n          id\r\n        }\r\n    }\r\n}";
        public const string GetRegions = "query($countryId:String!){\r\n    regions(countryId: $countryId){\r\n        nodes{\r\n          code,\r\n          name,\r\n          id\r\n        }\r\n    }\r\n}";

        #endregion

        #region Vehicles
        public const string GetVehicleUsages = "query{\r\n    vehicleUsages{\r\n        nodes{\r\n          id,\r\n          name,\r\n          code\r\n        }\r\n    }\r\n}";
        public const string GetVehicleTypes = "query{\r\n    vehicleTypes{\r\n        nodes{\r\n          id,\r\n          name,\r\n          code\r\n        }\r\n    }\r\n}";
        public const string GetVehicleBrands = "query{\r\n    vehicleBrands{\r\n        nodes{\r\n          id,\r\n          name,\r\n          code\r\n        }\r\n    }\r\n}";
        public const string GetVehicleModels = "query($brandId:String!){\r\n    vehicleModels(brandId: $brandId){\r\n        nodes{\r\n          id,\r\n          name,\r\n          code\r\n        }\r\n    }\r\n}";
        public const string GetUserVehicles = "query($userId: String!){\r\n    vehicles(userId: $userId, first: 100){\r\n        nodes{\r\n      id,\r\n      registrationCertificateNumber,\r\n      plateNumber,\r\n      userId,\r\n      modelId,\r\n      model,\r\n      brand,\r\n      brandId,\r\n      vehicleType,\r\n      vehicleTypeId,\r\n      vehicleUsage,\r\n      vehicleUsageId,\r\n      firstRegistrationDate,\r\n      vin,\r\n      bodyType,\r\n      places,\r\n      color,\r\n      engineType,\r\n      vehicleKilowatts,\r\n      steeringWheel,\r\n      engineVolume,\r\n      batteryCapacity,\r\n      netWeight,\r\n      grossWeight\r\n        }\r\n    }\r\n}";
        public const string GetVehicleById = "query($vehicleId:String!){\r\n    vehicleByVehicleId(vehicleId: $vehicleId){\r\n        nodes{\r\n    id,\r\n      registrationCertificateNumber,\r\n      plateNumber,\r\n      userId,\r\n      modelId,\r\n      model,\r\n      brand,\r\n      brandId,\r\n      vehicleType,\r\n      vehicleTypeId,\r\n      vehicleUsage,\r\n      vehicleUsageId,\r\n      firstRegistrationDate,\r\n      vin,\r\n      bodyType,\r\n      places,\r\n      color,\r\n      engineType,\r\n      vehicleKilowatts,\r\n      steeringWheel,\r\n      engineVolume,\r\n      batteryCapacity,\r\n      netWeight,\r\n      grossWeight\r\n        }\r\n    }\r\n}";
        #endregion

        #region Questions

        public const string GetMyThingsInsuranceQuestions = "query($myThingsInsuranceId: String!){\r\n    questions(myThingsInsuranceId: $myThingsInsuranceId){\r\n       answer,\r\n       questionId\r\n    }\r\n}";

        #endregion
    }
}
