using CovrMe.WebAPI.Models.Request.Insurances;
using CovrMe.WebAPI.Models.Request.Insurances.Casco;
using CovrMe.WebAPI.Models.Request.Insurances.CivilInsurance;
using CovrMe.WebAPI.Models.Request.Insurances.Health;
using CovrMe.WebAPI.Models.Request.Insurances.Mountain;
using CovrMe.WebAPI.Models.Request.Insurances.MyThings;
using CovrMe.WebAPI.Models.Request.Insurances.Travel;
using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Models.Result.Insurances.Casco;
using CovrMe.WebAPI.Models.Result.Insurances.Health;
using CovrMe.WebAPI.Models.Result.Insurances.Mountain;
using CovrMe.WebAPI.Models.Result.Insurances.MyThings;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.CivilInsurance;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Health;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Mountain;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.MyThings;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Travel;
using CovrMe.WebAPI.Models.Result.Insurances.Travel;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IInsuranceService
    {
        Task<CivilInsuranceSearchPayload> CivilInsuranceSearch(CivilInsuranceSearchInput req);
        Task<CivilInsuranceLongSearchPayload> CivilInsuranceLongSearch(CivilInsuranceLongSearchInput req);
        Task<CivilInsurancePolicyPayload> CivilInsurancePolicy(CivilInsurancePolicyInput req);

        Task<string> CreateInsurance(DateTime startDate, DateTime endDate, byte status, string insuranceCompanyId,
            decimal price, byte type, string currencyId, DateTime currentEndDate, string policyNo, int installments,
            string pdfUrl, int installmentToPay, string receiptUrl);

        Task<string> CreateCivilInsurance(decimal firstInstallmentPrice, decimal secondInstallmentPrice,
            decimal thirdInstallmentPrice, decimal fourthInstallmentPrice, string greenCardId,
            string stickerId, string insuranceId, DateTime? secondInstallmentDate, DateTime? thirdInstallmentDate,
            DateTime? fourthInstallmentDate, string greenCardPdfUrl, string usualDriverId, string userId,
            string vehicleId, decimal firstInstallmentTax, decimal secondInstallmentTax, decimal thirdInstallmentTax,
            decimal fourthInstallmentTax, string vehicleModel, string vehicleBrand, string vehiclePlateNumber);
        Task<string> CreateUsersInsurances(string userId, string insuranceId, bool isInsurer, bool isMainUser, bool isUsualDriver, bool isInsured);
        InsuranceModel GetInsuranceById(string id);
        List<InsuranceModel> GetAllUserInsurances(string userId);
        AllInsurancesPayload GetAllInsurances(AllInsurancesInput input);
        int GetAllInsurancesCount();
        int GetInsurancesCountDateRange(DateTime startDate, DateTime endDate);
        AllInsurancesSumUpModel GetAllInsurancesSumUp();
        Task<List<InsuranceModel>> GetInsurancesForPage(InsurancesForPageInput insurancesForPageInput);
        int GetCivilInsurancesCount();
        int GetCivilInsurancesCountByCompanyName(string companyName);
        Task<string> CreateInsurancePaymentInformation(string insuranceId, string paymentInformationId);
        Task<string> UpdateInsurance(string insuranceId, DateTime currentEndDate, int installmentToPay);
        Task<string> UpdateCivilInsurance(string civilInsuranceId, string greenCardPdf, string stickerId, string greenCardId);
        Task<CivilInsuranceInstallmentPayload> CivilInsuranceInstallment(CivilInsuranceInstallmentInput req);
        byte[] GetInsurancePolicy(string userId, string insuranceId);
        CheckVehicleCivilInsuranceAllowedPayload CheckVehicleNewCivilInsuranceAllowed(string vehiclePlateNumber);
        Task<TravelCalculationResultModel> UniqaTravelCalculation(TravelCalculationInput req);
        Task<TravelPolicyPayload> UniqaTravelPolicy(TravelPolicyInput req);
        Task<string> CreateTravelInsurance(string insuranceId, byte tripPurpose, byte territory, decimal limit);
        byte[] GetCivilInsuranceGreenCard(string userId, string insuranceId);
        Task<MountainCalculationResultModel> UniqaMountainCalculation(MountainCalculationInput req);
        Task<MountainPolicyPayload> UniqaMountainPolicy(MountainPolicyInput req);
        Task<string> CreateMountainInsurance(string insuranceId, bool isExtreme, int insuranceSum);
        Task<HealthCalculationResultModel> UniqaHealthCalculation(HealthCalculationInput req);
        Task<string> CreateHealthInsurance(string insuranceId, byte packageType, decimal installmentPrice, bool isFamily);
        Task<HealthPolicyPayload> UniqaHealthPolicy(HealthPolicyInput req);
        Task<MyThingsCalculationResultModel> UniqaMyThingsCalculation(MyThingsCalculationInput req);
        Task<string> CreateMyThingsInsurance(string insuranceId, byte propertyType, byte objectType, decimal insuranceSum, string brand, string model);
        Task<MyThingsPolicyPayload> UniqaMyThingsPolicy(MyThingsPolicyInput req);
        Task<HealthInsuranceInstallmentPayload> HealthInsuranceInstallment(HealthInsuranceInstallmentInput req);
        byte[] GetInsuranceReceipt(string userId, string insuranceId);
        Task<CivilInsuranceLongPolicyPayload> CivilInsuranceLongPolicy(CivilInsuranceLongPolicyInput req);
        Task<SendEmailCascoRequestPayload> SendEmailCascoRequest(CascoRequestEmailInput req);
        Task<byte[]> GetCivilInsuranceDocFile(string hash);
        int SaveFile(string url, byte[] file);
    }
}
