using CovrMe.WebAPI.Models.Request.Sirma;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.InstallmentPay;
using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy;
using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Sirma.Contracts
{
    public interface ISirmaCivilInsuranceService
    {
        Task<CivilInsuranceSearchResponse> CivilInsuranceSearch(CivilInsuranceSearchRequestModel req, int? installments);
        Task<CivilInsurancePolicyResponse> CivilInsurancePolicy(CivilInsurancePolicyRequestModel req);
        Task<CivilInsurancePolicyResponse> CivilInsuranceLongPolicy(CivilInsuranceLongPolicyRequestModel req);
        Task<CivilInsuranceInstallmentPayResponse> CivilInsuranceInstallmentPay(CivilInsuranceInstallmentRequestModel req);
        Task<byte[]> CivilInsuranceGetPdf(string hash);
        Task<CivilInsuranceSearchResponse> CivilInsuranceLongSearch(CivilInsuranceLongSearchRequestModel req, int? installments);

        //Task<CivilInsuranceSearchResponse> PerformTest(CivilInsuranceSearchRequestModel req);
        //Task<CivilInsuranceSearchResponse> PerformLongSearchTest(CivilInsuranceLongSearchRequestModel req);
    }
}
