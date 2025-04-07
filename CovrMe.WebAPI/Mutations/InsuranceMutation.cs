using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Request.Insurances;
using CovrMe.WebAPI.Models.Request.Insurances.Casco;
using CovrMe.WebAPI.Models.Request.Insurances.CivilInsurance;
using CovrMe.WebAPI.Models.Request.Insurances.Health;
using CovrMe.WebAPI.Models.Request.Insurances.Mountain;
using CovrMe.WebAPI.Models.Request.Insurances.MyThings;
using CovrMe.WebAPI.Models.Request.Insurances.Travel;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result.Insurances.Casco;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.CivilInsurance;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Health;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Mountain;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.MyThings;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Travel;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Vehicles;
using CovrMe.WebAPI.Services.LocalServices.Contracts;

namespace CovrMe.WebAPI.Mutations
{
    [ExtendObjectType(typeof(BaseMutation))]
    public class InsuranceMutation
    {
        [Authorize]
        public async Task<CivilInsuranceSearchPayload> CivilInsuranceSearch([Service] IInsuranceService insuranceService, CivilInsuranceSearchInput input)
        {
            var result = await insuranceService.CivilInsuranceSearch(input);

            return result;
        }

        [Authorize]
        public async Task<CivilInsuranceLongSearchPayload> CivilInsuranceLongSearch([Service] IInsuranceService insuranceService, CivilInsuranceLongSearchInput input)
        {
            var result = await insuranceService.CivilInsuranceLongSearch(input);

            return result;
        }

        [Authorize]
        public async Task<CivilInsurancePolicyPayload> CivilInsurancePolicy([Service] IInsuranceService insuranceService, CivilInsurancePolicyInput input)
        {
            var result = await insuranceService.CivilInsurancePolicy(input);

            return result;
        }

        [Authorize]
        public async Task<CivilInsuranceLongPolicyPayload> CivilInsuranceLongPolicy([Service] IInsuranceService insuranceService, CivilInsuranceLongPolicyInput input)
        {
            var result = await insuranceService.CivilInsuranceLongPolicy(input);

            return result;
        }

        [Authorize]
        public async Task<UserInsurancesPayload> UserInsurances([Service] IInsuranceService insuranceService, UserInsurancesInput input)
        {
            var result = new UserInsurancesPayload();
            var insurances = insuranceService.GetAllUserInsurances(input.UserId);

            result.Insurances = insurances;

            return result;
        }

        [Authorize]
        public async Task<AllInsurancesPayload> AllInsurances([Service] IInsuranceService insuranceService, AllInsurancesInput allInsurancesInput)
        {
            var insurances = insuranceService.GetAllInsurances(allInsurancesInput);

            return insurances;
        }

        [Authorize]
        public async Task<InsurancesForPagePayload> InsurancesForPage([Service] IInsuranceService insuranceService, InsurancesForPageInput insurancesForPageInput)
        {
            var result = new InsurancesForPagePayload();
            var insurances = await insuranceService.GetInsurancesForPage(insurancesForPageInput);

            result.Insurances = insurances;

            return result;
        }

        [Authorize]
        public async Task<CivilInsuranceInstallmentPayload> CivilInsuranceInstallment([Service] IInsuranceService insuranceService, CivilInsuranceInstallmentInput input)
        {
            var insurances = await insuranceService.CivilInsuranceInstallment(input);

            return insurances;
        }

        [Authorize]
        public async Task<CheckVehicleCivilInsuranceAllowedPayload> CheckVehicleCivilInsuranceAllowed([Service] IInsuranceService insuranceService, CheckVehicleCivilInsuranceAllowedInput input)
        {
            var result = insuranceService.CheckVehicleNewCivilInsuranceAllowed(input.VehiclePlateNumber);
            return result;
        }

        [Authorize]
        public async Task<TravelCalculationPayload> TravelCalculation([Service] IInsuranceService insuranceService, TravelCalculationInput input)
        {
            var result = new TravelCalculationPayload();

            var uniqaResult = await insuranceService.UniqaTravelCalculation(input);

            result.CalculationResult.Add(uniqaResult);
            return result;
        }

        [Authorize]
        public async Task<TravelPolicyPayload> TravelPolicy([Service] IInsuranceService insuranceService, TravelPolicyInput input)
        {
            var result = await insuranceService.UniqaTravelPolicy(input);
            return result;
        }

        [Authorize]
        public async Task<MountainCalculationPayload> MountainCalculation([Service] IInsuranceService insuranceService, MountainCalculationInput input)
        {
            var result = new MountainCalculationPayload();

            var uniqaResult = await insuranceService.UniqaMountainCalculation(input);

            result.CalculationResult.Add(uniqaResult);
            return result;
        }

        [Authorize]
        public async Task<MountainPolicyPayload> MountainPolicy([Service] IInsuranceService insuranceService, MountainPolicyInput input)
        {
            var result = await insuranceService.UniqaMountainPolicy(input);
            return result;
        }

        [Authorize]
        public async Task<HealthCalculationPayload> HealthCalculation([Service] IInsuranceService insuranceService, HealthCalculationInput input)
        {
            var result = new HealthCalculationPayload();

            var uniqaResult = await insuranceService.UniqaHealthCalculation(input);

            result.CalculationResult.Add(uniqaResult);
            return result;
        }

        [Authorize]
        public async Task<HealthPolicyPayload> HealthPolicy([Service] IInsuranceService insuranceService, HealthPolicyInput input)
        {
            var result = await insuranceService.UniqaHealthPolicy(input);
            return result;
        }

        [Authorize]
        public async Task<MyThingsCalculationPayload> MyThingsCalculation([Service] IInsuranceService insuranceService, MyThingsCalculationInput input)
        {
            var result = new MyThingsCalculationPayload();

            var uniqaResult = await insuranceService.UniqaMyThingsCalculation(input);

            result.CalculationResult.Add(uniqaResult);
            return result;
        }

        [Authorize]
        public async Task<MyThingsPolicyPayload> MyThingsPolicy([Service] IInsuranceService insuranceService, MyThingsPolicyInput input)
        {
            var result = await insuranceService.UniqaMyThingsPolicy(input);
            return result;
        }

        [Authorize]
        public async Task<HealthInsuranceInstallmentPayload> HealthInsuranceInstallment([Service] IInsuranceService insuranceService, HealthInsuranceInstallmentInput input)
        {
            var result = await insuranceService.HealthInsuranceInstallment(input);
            return result;
        }

        [Authorize]
        public async Task<SendEmailCascoRequestPayload> SendEmailCascoRequest([Service] IInsuranceService insuranceService, InsuranceUserEmailModelInput userInfo, InsuranceVehicleEmailModelInput vehicleInfo)
        {                                    
            var input = new CascoRequestEmailInput { UserInfo = userInfo, VehicleInfo = vehicleInfo};
            var result = await insuranceService.SendEmailCascoRequest(input);

            return result;
        }
    }
}
