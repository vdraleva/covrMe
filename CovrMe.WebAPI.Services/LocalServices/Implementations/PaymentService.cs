using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Models.Request.Dsk;
using CovrMe.WebAPI.Models.Request.Payment;
using CovrMe.WebAPI.Models.Result.Documents;
using CovrMe.WebAPI.Models.Result.Dsk;
using CovrMe.WebAPI.Models.Result.Payment;
using CovrMe.WebAPI.Models.Result.Payment.Payloads;
using CovrMe.WebAPI.Models.Result.Settings;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Services.Dsk.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Shared.Helpers;
using CovrMe.WebAPI.Shared.Mapping;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class PaymentService : IPaymentService
    {
        private IRepository<PaymentInformation> _paymentInfoRepository;
        private readonly ILogger _logger;
        private int _awaitingStatus = 8;
        private IUserService _userService;
        private ISettingsService _settingsService;
        private IDskPaymentService _dskPaymentService;
        private IInsuranceCompanyService _insuranceCompanyService;
        private IDocumentService _documentService;
        private IErrorService _errorService;

        public PaymentService(ILogger<PaymentService> logger, IRepository<PaymentInformation> paymentInfoRepository, IUserService userService,
            IDskPaymentService dskPaymentService, ISettingsService settingsService, IInsuranceCompanyService insuranceCompanyService,
            IDocumentService documentService, IErrorService errorService)
        {
            this._logger = logger;
            this._paymentInfoRepository = paymentInfoRepository;
            _userService = userService;
            this._dskPaymentService = dskPaymentService;
            _settingsService = settingsService;
            _insuranceCompanyService = insuranceCompanyService;
            _documentService = documentService;
            _errorService = errorService;
        }


        public async Task<string> CreatePaymentInfoAsync(string localOrderNumber, string dskOrderNumber)
        {
            string result = string.Empty;
            var info = this._paymentInfoRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.LocalOrderNumber == localOrderNumber);

            var awaitingStatus = (PaymentStatusEnum)this._awaitingStatus;
            if (info == null)
            {
                info = new PaymentInformation
                {
                    Id = Guid.NewGuid(),
                    LocalOrderNumber = localOrderNumber,
                    DskOrderNumber = dskOrderNumber,
                    Status = (int)GeneralStatusEnum.Unsuccess,
                    Operation = awaitingStatus.ToString()
                };

                await this._paymentInfoRepository.AddAsync(info);
                await this._paymentInfoRepository.SaveChangesAsync();

                result = info.Id.ToString();
            }

            return result;
        }

        public async Task<string> UpdatePaymentInfo(string dskOrderNumber, int status, string operation)
        {
            string result = string.Empty;
            var info = this._paymentInfoRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.DskOrderNumber == dskOrderNumber);

            if (info != null)
            {
                info.Status = status;
                info.Operation = operation;

                this._paymentInfoRepository.Update(info);
                await this._paymentInfoRepository.SaveChangesAsync();
                result = info.Id.ToString();
            }

            return result;
        }

        public T GetPaymentInfoByOrderNumber<T>(string localOrderNumber)
        {
            var info = this._paymentInfoRepository
                .AllAsNoTracking()
                .Where(x => x.LocalOrderNumber == localOrderNumber)
                .To<T>()
                .FirstOrDefault();

            return info;
        }

        public async Task<CreatePaymentPayload> CreatePayment(CreatePaymentInput input)
        {
            var result = new CreatePaymentPayload();

            try
            {
                var paymentResponse = new CreatePaymentResultModel();
                var civilInsuranceDocs = new CivilInsuranceDocumentsResultModel();

                string email = string.Empty;
                string phone = string.Empty;

                if (!string.IsNullOrEmpty(input.UserId))
                {
                    var userInfo = this._userService.GetCurrentUserById(input.UserId);

                    if (userInfo != null)
                    {
                        phone = userInfo.PhoneNumber;
                        email = userInfo.Email;
                    }
                }

                var request = new DskPaymentRequestModel();

                long amount = (long)(input.Amount * 100);
                request.Amount = amount;
                request.Currency = GlobalConstants.BgnCode;
                request.Language = GlobalConstants.LanguageCode;
                request.OrderNumber = await this.GetLocalOrderNumber();
                request.Email = email;
                request.Phone = phone;
                request.UserId = input.UserId;

                if (input.InsuranceType == (int)InsuranceTypeEnum.Civil)
                {
                    civilInsuranceDocs = await GetCivilInsuranceDocs(input.InsuranceCompanyName);
                    request.Description = input.VehiclePlateNumber;
                }

                var orderPaymentResponse = await this._dskPaymentService.DskPayment(request);

                if (orderPaymentResponse != null && !string.IsNullOrEmpty(orderPaymentResponse.OrderId))
                {
                    var createPaymentInfoResut = await this.CreatePaymentInfoAsync(request.OrderNumber, orderPaymentResponse.OrderId);

                    if (!string.IsNullOrEmpty(createPaymentInfoResut))
                    {
                        paymentResponse.Code = (int)GeneralStatusEnum.Success;
                        paymentResponse.LocalOrderNumber = request.OrderNumber;
                        paymentResponse.FormUrl = orderPaymentResponse.FormUrl;

                        if (!string.IsNullOrEmpty(civilInsuranceDocs.BatchId))
                        {
                            paymentResponse.DocumentBatchId = civilInsuranceDocs.BatchId;
                            paymentResponse.StickerId = civilInsuranceDocs.StickerId;
                            paymentResponse.GreencardId = civilInsuranceDocs.GreenCardId;
                        }
                    }
                    else
                    {
                        paymentResponse.Code = (int)GeneralStatusEnum.Unsuccess;
                    }
                }
                else
                {
                    paymentResponse.Code = (int)GeneralStatusEnum.Unsuccess;
                }

                result.LocalOrderNumber = paymentResponse.LocalOrderNumber;
                result.FormUrl = paymentResponse.FormUrl;
                result.DocumentBatchId = paymentResponse.DocumentBatchId;
                result.StickerId = paymentResponse.StickerId;
                result.GreencardId = paymentResponse.GreencardId;
                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                return result;
            }

        }

        public async Task<CheckPaymentStatusPayload> CheckPaymentStatus(string localOrderNumberId)
        {
            var result = new CheckPaymentStatusPayload();

            var info = this._paymentInfoRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.LocalOrderNumber == localOrderNumberId);

            if (info != null)
            {
                result.LocalOrderNumber = info.LocalOrderNumber;
                result.Status = info.Status;
                result.Id = info.Id.ToString();
                result.Operation = info.Operation;
            }

            return result;
        }

        private async Task<string> GetLocalOrderNumber()
        {
            string result = string.Empty;
            var info = this._settingsService.GetSettingByCode<SettingModel>(GlobalConstants.OrderNumbers);

            if (info != null)
            {
                result = info.Value;
                var parsedValue = int.Parse(info.Value);
                string newValue = (parsedValue + 1).ToString();

                await this._settingsService.UpdateSettingValue(info.Id, newValue);
            }

            return result;
        }

        private async Task<CivilInsuranceDocumentsResultModel> GetCivilInsuranceDocs(string insuranceCompany)
        {
            var model = new CivilInsuranceDocumentsResultModel();
            var civilInsuranceDocs = await this._documentService.GetCivilDocumentByInsuranceCompanyName(insuranceCompany);

            if (civilInsuranceDocs != null)
            {
                return civilInsuranceDocs;
            }
            else
            {
                return model;
            }
        }

        private void LogError(string exMessage, string trace)
        {
            string message = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm")} {trace} - {exMessage}";
            string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy")}.txt";

            this._errorService.LogError(message, fileName);
        }
    }
}
