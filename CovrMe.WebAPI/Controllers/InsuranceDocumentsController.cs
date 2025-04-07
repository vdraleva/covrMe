using CovrMe.WebAPI.Models.Request.Payment;
using CovrMe.WebAPI.Models.Result.Payment;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Models.Request.Insurances;
using Microsoft.AspNetCore.Authorization;
using GreenDonut;

namespace CovrMe.WebAPI.Controllers
{
    public class InsuranceDocumentsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IInsuranceService _insuranceService;
        public InsuranceDocumentsController(ILogger<InsuranceDocumentsController> logger, IInsuranceService insuranceService)
        {
            _logger = logger;
            this._insuranceService = insuranceService;
        }

        [HttpPost]
        [RequireHttps]
        [Route(GlobalConstants.GetPolicy)]
        [Authorize]
        public async Task<IActionResult> GetPolicy([FromBody] GetPolicyRequestModel req)
        {
            var file = this._insuranceService.GetInsurancePolicy(req.UserId, req.InsuranceId);

            string policyNo = req.PolicyNo.Replace('_', '/');
            string fileName = $"{policyNo}.pdf";
            return File(file, "application/pdf", fileName);
        }

        [HttpPost]
        [RequireHttps]
        [Route(GlobalConstants.GetReceipt)]
        [Authorize]
        public async Task<IActionResult> GetReceipt([FromBody] GetPolicyRequestModel req)
        {
            var file = this._insuranceService.GetInsuranceReceipt(req.UserId, req.InsuranceId);

            string policyNo = req.PolicyNo.Replace('_', '/');
            string fileName = $"{policyNo}-receipt.pdf";
            return File(file, "application/pdf", fileName);
        }

        [HttpPost]
        [RequireHttps]
        [Route(GlobalConstants.GetGreenCard)]
        [Authorize]
        public async Task<IActionResult> GetGreenCard([FromBody] GetPolicyRequestModel req)
        {
            var file = this._insuranceService.GetCivilInsuranceGreenCard(req.UserId, req.InsuranceId);

            string policyNo = req.PolicyNo.Replace('_', '/');
            string fileName = $"{policyNo}-greenCard.pdf";
            return File(file, "application/pdf", fileName);
        }

        [HttpGet]
        [RequireHttps]
        [Route("api/insuranceDocuments/GetDocFromSirma")]
        public async Task<IActionResult> GetDocFromSirma(string hash)
        {
            var byteArr = await this._insuranceService.GetCivilInsuranceDocFile(hash);
            var saveResult = this._insuranceService.SaveFile("C:\\Users\\SI\\Desktop\\policy\\receipt.pdf", byteArr);

            return Ok();

        }
    }
}
