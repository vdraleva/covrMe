using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.CivilInsurance.Request;
using CovrMe.Models.Insurances.Request;
using CovrMe.Models.Insurances.Request.Casco;
using CovrMe.Models.Insurances.Request.CivilInsurances;
using CovrMe.Models.Insurances.Request.HealthInsurance;
using CovrMe.Models.Insurances.Request.MountainInsurance;
using CovrMe.Models.Insurances.Request.MyThings;
using CovrMe.Models.Insurances.Request.TravelInsurance;
using CovrMe.Models.Insurances.Result;
using CovrMe.Models.Insurances.Result.CivilInsurances;
using CovrMe.Models.Insurances.Result.Payloads;
using CovrMe.Models.Insurances.Result.TravelInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Services.Contracts
{
    public interface IInsuranceService
    {
        Task<CivilInsuranceSearchResultModel> CivilInsuranceSearch(CivilInsuranceSearchInput civilInsuranceSearchInput, HttpClient client, string jwt);
        Task<CivilInsuranceSearchResultModel> CivilInsuranceLongSearch(CivilInsuranceLongSearchInput civilInsuranceSearchInput, HttpClient client, string jwt);
        Task<bool> UpdateCivilInsuranceDocumentStatus(string docId, string jwt, HttpClient client);
        Task<CivilInsurancePolicyResultModel> CivilInsurancePolicy(CivilInsurancePolicyInput civilInsurancePolicyInput, HttpClient client, string jwt);
        Task<CivilInsurancePolicyResultModel> CivilInsuranceLongPolicy(CivilInsuranceLongPolicyInput civilInsuranceLongPolicyInput, HttpClient client, string jwt);

        Task<List<InsuranceModel>> UserInsurances(UserInsurancesInput userInsurancesInput, string jwt, HttpClient client);
        Task<BaseResultModel> CivilInsuranceInstallmentPay(CivilInsuranceInstallmentInput civilInsuranceInstallmentInput, string jwt, HttpClient client);
        Task<Stream> GetPolicyPdf(string userId, string insuranceId, string policyNo, string jwt, HttpClient client);

        Task<CheckVehicleCivilInsuranceAllowedResultModel> CheckVehicleCivilInsuranceAllowed(string vehiclePlateNumber, string jwt, HttpClient client);
        Task<Stream> GetCivilInsuranceGreenCardPdf(string userId, string insuranceId, string policyNo, string jwt, HttpClient client);
        Task<List<CalculationModel>> TravelCalculation(TravelCalculationInput travelCalculationInput, string jwt, HttpClient client);

        Task<PolicyResultModel> TravelPolicy(TravelPolicyInput travelPolicyInput, string jwt, HttpClient client);
        Task<List<CalculationModel>> MountainCalculation(MountainCalculationInput mountainCalculationInput, string jwt, HttpClient client);
        Task<PolicyResultModel> MountainPolicy(MountainPolicyInput mountainPolicyInput, string jwt, HttpClient client);
        Task<List<CalculationInstallmentsModel>> HealthCalculation(HealthCalculationInput healthCalculationInput, string jwt, HttpClient client);
        Task<PolicyResultModel> HealthPolicy(HealthPolicyInput healthPolicyInput, string jwt, HttpClient client);
        Task<List<CalculationModel>> MyThingsCalculation(MyThingsCalculationInput myThingsCalculationInput, string jwt, HttpClient client);
        Task<PolicyResultModel> MyThingsPolicy(MyThingsPolicyInput myThingsPolicyInput, string jwt, HttpClient client);
        Task<List<QuestionModel>> GetMyThingsInsuranceQuestions(string myThingsInsuranceId, string jwt, HttpClient client);
        Task<BaseResultModel> HealthInsuranceInstallment(HealthInsuranceInstallmentInput healthInsuranceInstallmentInput, string jwt, HttpClient client);
        Task<Stream> GetReceiptPdf(string userId, string insuranceId, string policyNo, string jwt, HttpClient client);
        Task<BaseResultModel> SendEmailCasco(CascoRequestEmailInput input, HttpClient client, string jwt);
    }
}
