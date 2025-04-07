using HotChocolate.Subscriptions;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Models.Request.Payment;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Payment;
using CovrMe.WebAPI.Seeding.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CovrMe.WebAPI.Controllers
{
    public class PaymentController : Controller
    {
        private ITopicEventSender _sender;
        private IPaymentService _paymentService;
        private IInsuranceCompanyService _insuranceCompanyService;
        private int _depositedStatus = 1;
        private readonly ILogger _logger;
        public PaymentController(ITopicEventSender sender, IPaymentService paymentService, ILogger<PaymentController> logger,
            IInsuranceCompanyService insuranceCompanyService)
        {
            this._sender = sender;
            this._paymentService = paymentService;
            _logger = logger;
            _insuranceCompanyService = insuranceCompanyService;
        }

        [HttpGet]
        [RequireHttps]
        [Route(GlobalConstants.CheckPaymentStatus)]
        public async Task<IActionResult> CheckPaymentStatus([FromQuery] DskPaymentStatusRequestModel req)
        {
            var resultModel = new BaseResultModel();
            var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.OrderNumber);

            var approvedStatus = PaymentStatusEnum.approved;
            var depositedStatus = PaymentStatusEnum.deposited;

            if (paymentInfo != null && paymentInfo.DskOrderNumber == req.MdOrder)
            {
                if ((approvedStatus.ToString() == req.Operation || depositedStatus.ToString() == req.Operation) && req.Status == (int)GeneralStatusEnum.Success)
                {
                    resultModel.Code = (int)GeneralStatusEnum.Success;
                    resultModel.Message = req.Operation;                   
                }
                else
                {
                    resultModel.Code = (int)GeneralStatusEnum.Unsuccess;
                    resultModel.Message = req.Operation;
                }

                await this._paymentService.UpdatePaymentInfo(paymentInfo.DskOrderNumber, resultModel.Code, resultModel.Message);
            }
                        
            //await _sender.SendAsync(paymentInfo.LocalOrderNumber, resultModel);

            return Ok("Updated");
        }
    }
}
