using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Result.InsuranceCompanies;
using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;

namespace CovrMe.WebAPI.Queries
{
    [ExtendObjectType(typeof(BaseQuery))]
    public class InsuranceQuery
    {
        [GraphQLName("insuranceCountByCompany")]
        [Authorize]
        public int GetCivilInsurancesCountByCompanyName([Service] IInsuranceService insuranceService, string companyName)
        {
            var insCount = insuranceService.GetCivilInsurancesCountByCompanyName(companyName);

            return insCount;
        }

        [GraphQLName("insCompanies")]
        [Authorize]
        public IQueryable<InsuranceCompanyModel> GetInsuranceCompanies([Service] IInsuranceCompanyService insuranceCompanyService)
        {
            var insuranceCompanies = insuranceCompanyService.GetCompanies<InsuranceCompanyModel>();

            return insuranceCompanies;
        }

        [GraphQLName("insurancesCount")]
        [Authorize]
        public int GetAllInsurancesCount([Service] IInsuranceService insuranceService)
        {
            var insCount = insuranceService.GetAllInsurancesCount();

            return insCount;
        }

        [GraphQLName("getInsurancesSumUp")]
        [Authorize]
        public AllInsurancesSumUpModel GetAllInsurancesSumUp([Service] IInsuranceService insuranceService)
        {
            var result = insuranceService.GetAllInsurancesSumUp();

            return result;
        }

        [GraphQLName("insurancesCountDateRange")]
        [Authorize]
        public int GetAllInsurancesCount([Service] IInsuranceService insuranceService, DateTime startDate, DateTime endDate)
        {
            var insCount = insuranceService.GetInsurancesCountDateRange(startDate, endDate);

            return insCount;
        }
    }
}
