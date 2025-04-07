using CovrMe.WebAPI.Models.Request.Insurances;
using CovrMe.WebAPI.Models.Request.Insurances.Health;
using CovrMe.WebAPI.Models.Request.Insurances.Mountain;
using CovrMe.WebAPI.Models.Request.Insurances.MyThings;
using CovrMe.WebAPI.Models.Request.Insurances.Travel;
using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Models.Result.Insurances.Health;
using CovrMe.WebAPI.Models.Result.Insurances.Mountain;
using CovrMe.WebAPI.Models.Result.Insurances.MyThings;
using CovrMe.WebAPI.Models.Result.Insurances.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniqaSoapService;

namespace CovrMe.WebAPI.Services.Uniqa.Contracts
{
    public interface IUniqaInsuranceService
    {
        Task<CalculateTravelPremiumResponse> TestSoap();
        Task<TravelCalculationResultModel> TravelCalculation(TravelCalculationInput req);
        Task<OfferResultModel> TravelOffer(TravelPolicyInput req);
        Task<TravelOrderInfoResultModel> TravelOrderInfo(string orderId);
        Task<IssuePolicyResultModel> IssueTravelPolicyRequest(string orderId);
        Task<MountainCalculationResultModel> MountainCalculation(MountainCalculationInput req);
        Task<OfferResultModel> MountainOffer(MountainPolicyInput req);
        Task<MountainOrderInfoResultModel> MountainOrderInfo(string orderId);
        Task<IssuePolicyResultModel> IssueMountainPolicyRequest(string orderId);
        Task<HealthCalculationResultModel> HealthCalculation(HealthCalculationInput req);
        Task<OfferResultModel> HealthOffer(HealthPolicyInput req);
        Task<HealthOrderInfoResultModel> HealthOrderInfo(string orderId);
        Task<IssuePolicyResultModel> IssueHealthPolicyRequest(string orderId);
        Task<MyThingsCalculationResultModel> MyThingsCalculation(MyThingsCalculationInput req);
        Task<OfferResultModel> MyThingsOffer(MyThingsPolicyInput req);
        Task<MyThingsOrderInfoResultModel> MyThingsOrderInfo(string orderId);
        Task<IssuePolicyResultModel> IssueMyThingsPolicyRequest(string orderId);
        Task<PayUniqaInstallmentResultModel> PayInstallment(PayUniqaInstallmentRequestModel req);
        Task<UniqaGetPolicyInstallmentInfo> GetPolicyInstallmentInfo(string policyNo, string insuranceType, bool insuranceGroup);
    }
}
