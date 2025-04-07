using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Models;
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
using CovrMe.WebAPI.Services.Caching.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Services.Sirma.Implementations;
using CovrMe.WebAPI.Services.Uniqa.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UniqaSoapService;
using static iTextSharp.text.pdf.AcroFields;

namespace CovrMe.WebAPI.Services.Uniqa.Implementations
{
    public class UniqaInsuranceService : IUniqaInsuranceService
    {
        public readonly string _serviceUrl;
        public readonly EndpointAddress _endpointAddress;
        public readonly BasicHttpBinding _basicHttpBinding;
        private readonly ILogger<UniqaInsuranceService> _logger;
        private IConfiguration _configuration;
        private IErrorService _errorService;

        private readonly string _baseUrl = string.Empty;
        private readonly string _certificatePFXPath = string.Empty;
        private string _agentId;
        private string _certName;

        public UniqaInsuranceService(ILogger<UniqaInsuranceService> logger,IConfiguration configuration, IErrorService errorService)
        {
            _logger = logger;
            _configuration = configuration;
            this._errorService = errorService;

            _certificatePFXPath = _configuration["UniqaCertificatesSettingsPaths:CertificatePFXPath"];
            _agentId = _configuration["UniqaAuthSettings:AgentId"];
            _certName = _configuration["UniqaCertificatesSettingsPaths:CertificateName"];
            _serviceUrl = _configuration["Routing:UniqaSoaplUrl"];
        }

        public async Task<CalculateTravelPremiumResponse> TestSoap()
        {
            PSMPolicyServiceClient service = CreateConnection();

            var risk = new List<TravelAddRisk>();
            risk.Add(new TravelAddRisk
            {
                RiskType = 96,
                Limit = 400
            });

            risk.Add(new TravelAddRisk
            {
                RiskType = 97,
                Limit = 1000
            });
            var travelCalculation = new TravelCalculation
            {
                AgentID = this._agentId,
                PolicyType = 9,
                StartDate = DateTime.Parse("02/14/2024"),
                EndDate = DateTime.Parse("02/25/2025"),
                TripPurpose = 1,
                Territory = 10,
                Limit = 2000,
                Currency = GlobalConstants.UniqaEuroCurrencyCode,
                NumberAge2 = 10,
                AdditionalRisk = risk.ToArray()
            };

            var request = new askTravelPremiumRequest();
            request.Calculation = travelCalculation;

            CalculateTravelPremiumResponse travelResult = await service.CalculateTravelPremiumAsync(request);

            return travelResult;
        }

        #region Travel

        public async Task<TravelCalculationResultModel> TravelCalculation(TravelCalculationInput req)
        {
            var result = new TravelCalculationResultModel();
            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var risk = this.AddRisk();

                var travelCalculation = new TravelCalculation
                {
                    AgentID = this._agentId,
                    PolicyType = GlobalConstants.TravelPolicyType,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    TripPurpose = req.TripPurpose,
                    Territory = req.Territory,
                    Limit = req.Limit,
                    Currency = GlobalConstants.UniqaEuroCurrencyCode,
                    NumberAge1 = req.NumberUnder18,
                    NumberAge2 = req.Number18To65,
                    NumberAge3 = req.Number66To70,
                    NumberAge4 = req.Number71To75,
                    NumberAge5 = req.Number76To80,
                    NumberAge6 = req.NumberOver80,
                    AdditionalRisk = risk.ToArray()
                };

                var request = new askTravelPremiumRequest();
                request.Calculation = travelCalculation;

                string requestJson = JsonConvert.SerializeObject(travelCalculation);
                string responseJson = string.Empty;

                CalculateTravelPremiumResponse travelResult = await service.CalculateTravelPremiumAsync(request);

                if (travelResult != null && travelResult.Body != null)
                {
                    if (travelResult.Body.CalculateTravelPremiumResult != null)
                    {
                        if (travelResult.Body.CalculateTravelPremiumResult.Status.StatusType == 0)
                        {
                            result.Id = travelResult.Body.CalculateTravelPremiumResult.CorrelationID;
                            result.PremiumWithoutTax = travelResult.Body.CalculateTravelPremiumResult.DuePremium;
                            result.Tax = travelResult.Body.CalculateTravelPremiumResult.StateTaxSum;
                            result.Premium = travelResult.Body.CalculateTravelPremiumResult.TotalDueAmount;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = travelResult.Body.CalculateTravelPremiumResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(travelResult.Body);

                            result.Message = travelResult.Body.CalculateTravelPremiumResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });
                            //throw new Exception(travelResult.Body.CalculateTravelPremiumResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<OfferResultModel> TravelOffer(TravelPolicyInput req)
        {
            var result = new OfferResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var risk = this.AddRisk();
                var clientData = this.GetClientData(req.Clients);
                var holder = this.GetHolderData(req.Holder);

                var policy = new TravelPolicy
                {
                    AgentID = this._agentId,
                    OfficeID = 0,
                    PolicyType = GlobalConstants.TravelPolicyType,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    TripPurpose = req.TripPurpose,
                    Territory = req.Territory,
                    Limit = req.Limit,
                    Currency = GlobalConstants.UniqaEuroCurrencyCode,
                    Clients = clientData.ToArray(),
                    AdditionalRisk = risk.ToArray(),
                    UserName = req.Username,
                    IssueLocation = GlobalConstants.UniqaPolicyIssueLocation,
                    Holder = holder
                };

                string requestJson = JsonConvert.SerializeObject(policy);
                string responseJson = string.Empty;

                var request = new calculateTravelRequest();
                request.Policy = policy;

                CalculateTravelTariffResponse offerResult = await service.CalculateTravelTariffAsync(request);

                if (offerResult != null && offerResult.Body != null)
                {
                    if (offerResult.Body.CalculateTravelTariffResult != null)
                    {
                        if (offerResult.Body.CalculateTravelTariffResult.Status.StatusType == 0)
                        {
                            result.OfferId = offerResult.Body.CalculateTravelTariffResult.OrderID;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = offerResult.Body.CalculateTravelTariffResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(offerResult.Body);
                            result.Message = offerResult.Body.CalculateTravelTariffResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });
                            //throw new Exception(offerResult.Body.CalculateTravelTariffResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<TravelOrderInfoResultModel> TravelOrderInfo(string orderId)
        {
            var result = new TravelOrderInfoResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var request = new getTravelOrderInfoRequest();
                request.OrderID = orderId;
                request.AgentID = this._agentId;

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                GetTravelOrderInfoResponse1 orderInfo = await service.GetTravelOrderInfoAsync(request);

                if (orderInfo != null && orderInfo.Body != null)
                {
                    if (orderInfo.Body.GetTravelOrderInfoResult != null)
                    {
                        if (orderInfo.Body.GetTravelOrderInfoResult.Status.StatusType == 0)
                        {
                            result.PolicuNo = orderInfo.Body.GetTravelOrderInfoResult.UPN;
                            result.PolicyPdf = orderInfo.Body.GetTravelOrderInfoResult.PolicyInPDF;
                            result.PremiumWithoutTax = orderInfo.Body.GetTravelOrderInfoResult.DuePremium;
                            result.Tax = orderInfo.Body.GetTravelOrderInfoResult.StateTaxSum;
                            result.Premium = orderInfo.Body.GetTravelOrderInfoResult.TotalDueAmount;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = orderInfo.Body.GetTravelOrderInfoResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(orderInfo.Body);
                            result.Message = orderInfo.Body.GetTravelOrderInfoResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });
                            //throw new Exception(orderInfo.Body.GetTravelOrderInfoResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<IssuePolicyResultModel> IssueTravelPolicyRequest(string orderId)
        {
            var result = new IssuePolicyResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var request = new issueTravelPolicyRequest();
                request.AgentID = this._agentId;
                request.OrderID = orderId;

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                IssueTravelPolicyResponse1 policyResult = await service.IssueTravelPolicyAsync(request);

                if (policyResult != null && policyResult.Body != null)
                {
                    if (policyResult.Body.IssueTravelPolicyResult != null)
                    {
                        if (policyResult.Body.IssueTravelPolicyResult.Status.StatusType == 0)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = policyResult.Body.IssueTravelPolicyResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(policyResult.Body);
                            result.Message = policyResult.Body.IssueTravelPolicyResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });
                            //throw new Exception(policyResult.Body.IssueTravelPolicyResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        #endregion

        #region Mountain

        public async Task<MountainCalculationResultModel> MountainCalculation(MountainCalculationInput req)
        {
            var result = new MountainCalculationResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var mountainCalculation = new AlpineCalculation
                {
                    AgentID = this._agentId,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    InsuranceSum = req.InsuranceSum,
                    NumberAge1 = req.NumberUnder18,
                    NumberAge2 = req.NumberOver18,
                    isExtreme = req.IsExtreme,
                };

                var request = new askAlpinePremiumRequest();
                request.Calculation = mountainCalculation;

                string requestJson = JsonConvert.SerializeObject(mountainCalculation);
                string responseJson = string.Empty;

                CalculateAlpinePremiumResponse mountainResult = await service.CalculateAlpinePremiumAsync(request);

                if (mountainResult != null && mountainResult.Body != null)
                {
                    if (mountainResult.Body.CalculateAlpinePremiumResult != null)
                    {
                        if (mountainResult.Body.CalculateAlpinePremiumResult.Status.StatusType == 0)
                        {
                            result.Id = mountainResult.Body.CalculateAlpinePremiumResult.CorrelationID;
                            result.PremiumWithoutTax = mountainResult.Body.CalculateAlpinePremiumResult.DuePremium;
                            result.Tax = mountainResult.Body.CalculateAlpinePremiumResult.StateTaxSum;
                            result.Premium = mountainResult.Body.CalculateAlpinePremiumResult.TotalDueAmount;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = mountainResult.Body.CalculateAlpinePremiumResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(mountainResult.Body);
                            result.Message = mountainResult.Body.CalculateAlpinePremiumResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });
                            //throw new Exception(mountainResult.Body.CalculateAlpinePremiumResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<OfferResultModel> MountainOffer(MountainPolicyInput req)
        {
            var result = new OfferResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var personData = this.GetPersonData(req.Clients);
                var holder = this.GetHolderData(req.Holder);

                var policy = new AlpinePolicy
                {
                    AgentID = this._agentId,
                    OfficeID = 0,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    UserName = req.Username,
                    IssueLocation = GlobalConstants.UniqaPolicyIssueLocation,
                    Holder = holder,
                    InsuranceSum = req.InsuranceSum,
                    isExtreme = req.IsExtreme,
                    Clients = personData.ToArray()
                };

                var request = new calculateAlpineRequest();
                request.Policy = policy;

                string requestJson = JsonConvert.SerializeObject(policy);
                string responseJson = string.Empty;

                CalculateAlpineTariffResponse offerResult = await service.CalculateAlpineTariffAsync(request);

                if (offerResult != null && offerResult.Body != null)
                {
                    if (offerResult.Body.CalculateAlpineTariffResult != null)
                    {
                        if (offerResult.Body.CalculateAlpineTariffResult.Status.StatusType == 0)
                        {
                            result.OfferId = offerResult.Body.CalculateAlpineTariffResult.OrderID;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = offerResult.Body.CalculateAlpineTariffResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(offerResult.Body);
                            result.Message = offerResult.Body.CalculateAlpineTariffResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });
                            //throw new Exception(offerResult.Body.CalculateAlpineTariffResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<MountainOrderInfoResultModel> MountainOrderInfo(string orderId)
        {
            var result = new MountainOrderInfoResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var request = new getOrderInfoRequest();
                request.OrderID = orderId;
                request.AgentID = this._agentId;

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                GetAlpineOrderInfoResponse orderInfo = await service.GetAlpineOrderInfoAsync(request);

                if (orderInfo != null && orderInfo.Body != null)
                {
                    if (orderInfo.Body.GetAlpineOrderInfoResult != null)
                    {
                        if (orderInfo.Body.GetAlpineOrderInfoResult.Status.StatusType == 0)
                        {
                            result.PolicuNo = orderInfo.Body.GetAlpineOrderInfoResult.UPN;
                            result.PolicyPdf = orderInfo.Body.GetAlpineOrderInfoResult.PolicyInPDF;
                            result.PremiumWithoutTax = orderInfo.Body.GetAlpineOrderInfoResult.DuePremium;
                            result.Tax = orderInfo.Body.GetAlpineOrderInfoResult.StateTaxSum;
                            result.Premium = orderInfo.Body.GetAlpineOrderInfoResult.TotalDueAmount;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = orderInfo.Body.GetAlpineOrderInfoResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(orderInfo.Body);
                            result.Message = orderInfo.Body.GetAlpineOrderInfoResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(orderInfo.Body.GetAlpineOrderInfoResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<IssuePolicyResultModel> IssueMountainPolicyRequest(string orderId)
        {
            var result = new IssuePolicyResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var request = new issuePolicyRequest();
                request.AgentID = this._agentId;
                request.OrderID = orderId;

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                IssueAlpinePolicyResponse policyResult = await service.IssueAlpinePolicyAsync(request);

                if (policyResult != null && policyResult.Body != null)
                {
                    if (policyResult.Body.IssueAlpinePolicyResult != null)
                    {
                        if (policyResult.Body.IssueAlpinePolicyResult.Status.StatusType == 0)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = policyResult.Body.IssueAlpinePolicyResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(policyResult.Body);
                            result.Message = policyResult.Body.IssueAlpinePolicyResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(policyResult.Body.IssueAlpinePolicyResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        #endregion

        #region Health

        public async Task<HealthCalculationResultModel> HealthCalculation(HealthCalculationInput req)
        {
            var result = new HealthCalculationResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var risk = this.AddRisk();

                ArrayOfDateTime dateTimes = new ArrayOfDateTime();
                dateTimes.AddRange(req.InsuredBirthDate);

                var healthCalculation = new HealthComfortCalculation
                {
                    AgentID = this._agentId,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    CurrencyID = 1,
                    PacketID = (byte)req.PacketId,
                    InstallmentCount = (byte)req.InstallmentCount,
                    isFamily = req.IsFamily,
                    InsuredBirthDate = dateTimes
                };

                var request = new askHealthComfortPremiumRequest();
                request.Calculation = healthCalculation;

                string requestJson = JsonConvert.SerializeObject(healthCalculation);
                string responseJson = string.Empty;

                CalculateHealthComfortPremiumResponse healthResult = await service.CalculateHealthComfortPremiumAsync(request);

                if (healthResult != null && healthResult.Body != null)
                {
                    if (healthResult.Body.CalculateHealthComfortPremiumResult != null)
                    {
                        if (healthResult.Body.CalculateHealthComfortPremiumResult.Status.StatusType == 0)
                        {
                            result.Id = healthResult.Body.CalculateHealthComfortPremiumResult.CorrelationID;
                            var installments = healthResult.Body.CalculateHealthComfortPremiumResult.Installments;

                            foreach (var installment in installments.Take(req.InstallmentCount))
                            {
                                var currentInstallment = new InstallmentModel
                                {
                                    Number = installment.Number,
                                    Date = installment.Date,
                                    Total = installment.Total,
                                    Tax = installment.Tax,
                                };

                                result.Installments.Add(currentInstallment);
                            }

                            result.Premium = result.Installments.Sum(x => x.Total + x.Tax);
                            result.PremiumWithoutTax = result.Installments.Sum(x => x.Total);
                            result.Tax = result.Installments.Sum(x => x.Tax);
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = healthResult.Body.CalculateHealthComfortPremiumResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(healthResult.Body);
                            result.Message = healthResult.Body.CalculateHealthComfortPremiumResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(healthResult.Body.CalculateHealthComfortPremiumResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<OfferResultModel> HealthOffer(HealthPolicyInput req)
        {
            var result = new OfferResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var risk = this.AddRisk();
                var clientData = this.GetPersonData(req.Clients);
                var holder = this.GetHolderData(req.Holder);
                var questions = this.GetQuestionnaire(req.Questionnaire);

                var policy = new HealthComfortPolicy
                {
                    AgentID = this._agentId,
                    OfficeID = 0,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    CurrencyID = 1,
                    Clients = clientData.ToArray(),
                    UserName = req.Username,
                    IssueLocation = GlobalConstants.UniqaPolicyIssueLocation,
                    Holder = holder,
                    PacketID = (byte)req.PacketId,
                    InstallmentCount = (byte)req.InstallmentCount,
                    Questionnaire = questions.ToArray(),
                    isFamily = req.IsFamily,
                };

                string requestJson = JsonConvert.SerializeObject(policy);
                string responseJson = string.Empty;

                var request = new calculateHealthComfortRequest();
                request.Policy = policy;

                CalculateHealthComfortTariffResponse offerResult = await service.CalculateHealthComfortTariffAsync(request);

                if (offerResult != null && offerResult.Body != null)
                {
                    if (offerResult.Body.CalculateHealthComfortTariffResult != null)
                    {
                        if (offerResult.Body.CalculateHealthComfortTariffResult.Status.StatusType == 0)
                        {
                            result.OfferId = offerResult.Body.CalculateHealthComfortTariffResult.OrderID;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = offerResult.Body.CalculateHealthComfortTariffResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(offerResult.Body);
                            result.Message = offerResult.Body.CalculateHealthComfortTariffResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(offerResult.Body.CalculateHealthComfortTariffResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<HealthOrderInfoResultModel> HealthOrderInfo(string orderId)
        {
            var result = new HealthOrderInfoResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var request = new getOrderInfoRequest();
                request.OrderID = orderId;
                request.AgentID = this._agentId;

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                GetHealthComfortOrderInfoResponse orderInfo = await service.GetHealthComfortOrderInfoAsync(request);

                if (orderInfo != null && orderInfo.Body != null)
                {
                    if (orderInfo.Body.GetHealthComfortOrderInfoResult != null)
                    {
                        if (orderInfo.Body.GetHealthComfortOrderInfoResult.Status.StatusType == 0)
                        {
                            result.PolicuNo = orderInfo.Body.GetHealthComfortOrderInfoResult.UPN;
                            result.PolicyPdf = orderInfo.Body.GetHealthComfortOrderInfoResult.PolicyInPDF;
                            result.PremiumWithoutTax = orderInfo.Body.GetHealthComfortOrderInfoResult.DuePremium;
                            result.Tax = orderInfo.Body.GetHealthComfortOrderInfoResult.StateTaxSum;

                            var installments = orderInfo.Body.GetHealthComfortOrderInfoResult.Installments;

                            foreach (var installment in installments)
                            {
                                var currentInstallment = new InstallmentModel
                                {
                                    Number = installment.Number,
                                    Date = installment.Date,
                                    Total = installment.Total,
                                    Tax = installment.Tax
                                };

                                result.Installments.Add(currentInstallment);
                            }
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = orderInfo.Body.GetHealthComfortOrderInfoResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(orderInfo.Body);
                            result.Message = orderInfo.Body.GetHealthComfortOrderInfoResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(orderInfo.Body.GetHealthComfortOrderInfoResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<IssuePolicyResultModel> IssueHealthPolicyRequest(string orderId)
        {
            var result = new IssuePolicyResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var request = new issuePolicyRequest();
                request.AgentID = this._agentId;
                request.OrderID = orderId;

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                IssueHealthComfortPolicyResponse policyResult = await service.IssueHealthComfortPolicyAsync(request);

                if (policyResult != null && policyResult.Body != null)
                {
                    if (policyResult.Body.IssueHealthComfortPolicyResult != null)
                    {
                        if (policyResult.Body.IssueHealthComfortPolicyResult.Status.StatusType == 0)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = policyResult.Body.IssueHealthComfortPolicyResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(policyResult.Body);
                            result.Message = policyResult.Body.IssueHealthComfortPolicyResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(policyResult.Body.IssueHealthComfortPolicyResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        #endregion

        #region MyThings

        public async Task<MyThingsCalculationResultModel> MyThingsCalculation(MyThingsCalculationInput req)
        {
            var result = new MyThingsCalculationResultModel();
            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var myThingsList = this.GetMyThings(req.PropertyTypeId, req.ObjectTypeId, req.InsuranceSum, req.Questionnaire);

                var myThingsCalculation = new MyThingsCalculation
                {
                    AgentID = this._agentId,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    CurrencyID = 1,
                    MyThingsList = myThingsList.ToArray(),

                };

                string requestJson = JsonConvert.SerializeObject(myThingsCalculation);
                string responseJson = string.Empty;

                var request = new askMyThingsPremiumRequest();
                request.Calculation = myThingsCalculation;

                CalculateMyThingsPremiumResponse myThingsResult = await service.CalculateMyThingsPremiumAsync(request);

                if (myThingsResult != null && myThingsResult.Body != null)
                {
                    if (myThingsResult.Body.CalculateMyThingsPremiumResult != null)
                    {
                        if (myThingsResult.Body.CalculateMyThingsPremiumResult.Status.StatusType == 0)
                        {
                            result.Id = myThingsResult.Body.CalculateMyThingsPremiumResult.CorrelationID;
                            result.PremiumWithoutTax = myThingsResult.Body.CalculateMyThingsPremiumResult.DuePremium;
                            result.Tax = myThingsResult.Body.CalculateMyThingsPremiumResult.StateTaxSum;
                            result.Premium = myThingsResult.Body.CalculateMyThingsPremiumResult.TotalDueAmount;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = myThingsResult.Body.CalculateMyThingsPremiumResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(myThingsResult.Body);
                            result.Message = myThingsResult.Body.CalculateMyThingsPremiumResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(myThingsResult.Body.CalculateMyThingsPremiumResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return result;
            }
        }

        public async Task<OfferResultModel> MyThingsOffer(MyThingsPolicyInput req)
        {
            var result = new OfferResultModel();
            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var holder = this.GetHolderData(req.Holder);
                var myThingsList = this.GetMyThings(req.PropertyTypeId,req.ObjectTypeId, req.InsuranceSum, req.Questionnaire);

                var propertyAddress = new Address
                {
                    Country = req.Holder.Country,
                    Location = req.Holder.Location,
                    AddressBul = req.Holder.Address
                };

                var policy = new MyThingsPolicy
                {
                    AgentID = this._agentId,
                    OfficeID = 0,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    CurrencyID = 1,
                    UserName = req.Username,
                    IssueLocation = GlobalConstants.UniqaPolicyIssueLocation,
                    Holder = holder,
                    PropertyAddress = propertyAddress,
                    InstallmentsCount = 1,
                    MyThingsList = myThingsList.ToArray()

                };

                string requestJson = JsonConvert.SerializeObject(policy);
                string responseJson = string.Empty;

                var request = new calculateMyThingsRequest();
                request.Policy = policy;

                CalculateMyThingsTariffResponse offerResult = await service.CalculateMyThingsTariffAsync(request);

                if (offerResult != null && offerResult.Body != null)
                {
                    if (offerResult.Body.CalculateMyThingsTariffResult != null)
                    {
                        if (offerResult.Body.CalculateMyThingsTariffResult.Status.StatusType == 0)
                        {
                            result.OfferId = offerResult.Body.CalculateMyThingsTariffResult.OrderID;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = offerResult.Body.CalculateMyThingsTariffResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(offerResult.Body);
                            result.Message = offerResult.Body.CalculateMyThingsTariffResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(offerResult.Body.CalculateMyThingsTariffResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return result;
            }
        }

        public async Task<MyThingsOrderInfoResultModel> MyThingsOrderInfo(string orderId)
        {
            var result = new MyThingsOrderInfoResultModel();
            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var request = new getOrderInfoRequest();
                request.OrderID = orderId;
                request.AgentID = this._agentId;

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                GetMyThingsOrderInfoResponse orderInfo = await service.GetMyThingsOrderInfoAsync(request);

                if (orderInfo != null && orderInfo.Body != null)
                {
                    if (orderInfo.Body.GetMyThingsOrderInfoResult != null)
                    {
                        if (orderInfo.Body.GetMyThingsOrderInfoResult.Status.StatusType == 0)
                        {
                            result.PolicuNo = orderInfo.Body.GetMyThingsOrderInfoResult.UPN;
                            result.PolicyPdf = orderInfo.Body.GetMyThingsOrderInfoResult.PolicyInPDF;
                            result.PremiumWithoutTax = orderInfo.Body.GetMyThingsOrderInfoResult.DuePremium;
                            result.Tax = orderInfo.Body.GetMyThingsOrderInfoResult.StateTaxSum;
                            result.Premium = orderInfo.Body.GetMyThingsOrderInfoResult.TotalDueAmount;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = orderInfo.Body.GetMyThingsOrderInfoResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(orderInfo.Body);
                            result.Message = orderInfo.Body.GetMyThingsOrderInfoResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(orderInfo.Body.GetMyThingsOrderInfoResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return result;
            }
        }

        public async Task<IssuePolicyResultModel> IssueMyThingsPolicyRequest(string orderId)
        {
            var result = new IssuePolicyResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();

                var request = new issuePolicyRequest();
                request.AgentID = this._agentId;
                request.OrderID = orderId;

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                IssueMyThingsPolicyResponse policyResult = await service.IssueMyThingsPolicyAsync(request);

                if (policyResult != null && policyResult.Body != null)
                {
                    if (policyResult.Body.IssueMyThingsPolicyResult != null)
                    {
                        if (policyResult.Body.IssueMyThingsPolicyResult.Status.StatusType == 0)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            result.ErrorId = errorId;
                            string correlationId = policyResult.Body.IssueMyThingsPolicyResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(policyResult.Body);
                            result.Message = policyResult.Body.IssueMyThingsPolicyResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });

                            //throw new Exception(policyResult.Body.IssueMyThingsPolicyResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return result;
            }
        }

        #endregion

        #region Common

        public async Task<PayUniqaInstallmentResultModel> PayInstallment(PayUniqaInstallmentRequestModel req)
        {
            var result = new PayUniqaInstallmentResultModel();

            try
            {
                PSMPolicyServiceClient service = CreateConnection();


                var request = new payInstallmentRequest
                {
                    AgentID = this._agentId,
                    InsuranceGroup = req.InsuranceGroup,
                    InsuranceType = req.InsuranceType,
                    PolicyNumber = req.PolicyNumber,
                    InstallmentNumber = (short)req.InstallmentNumber,
                    PaymentType = 1,
                    BankPaymentDate = DateTime.Now,
                    ReceiptUserCreated = req.ReceiptUserCreated,
                    ReceiptPayer = req.ReceiptPayer,
                    AttachmentNumber = (short)req.AttachmentNumber

                };

                string requestJson = JsonConvert.SerializeObject(request);
                string responseJson = string.Empty;

                PayPolicyInstallmentResponse installmentResult = await service.PayPolicyInstallmentAsync(request);

                if (installmentResult != null && installmentResult.Body != null)
                {
                    if (installmentResult.Body.PayPolicyInstallmentResult != null)
                    {
                        if (installmentResult.Body.PayPolicyInstallmentResult.Status.StatusType == 0)
                        {
                            result.ReceiptNumber = installmentResult.Body.PayPolicyInstallmentResult.ReceiptNumber;
                            result.ReceiptPdf = installmentResult.Body.PayPolicyInstallmentResult.ReceiptDocument;
                        }
                        else
                        {
                            string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                            int randomNumber = Helpers.GetRandomNumber();
                            string errorId = $"ins-{dateStr}-{randomNumber}";
                            //result.ErrorId = errorId;
                            string correlationId = installmentResult.Body.PayPolicyInstallmentResult.CorrelationID;
                            responseJson = JsonConvert.SerializeObject(installmentResult.Body);
                            //result.Message = policyResult.Body.IssueAlpinePolicyResult.Status.Info.FirstOrDefault().Description;

                            Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });
                            //throw new Exception(installmentResult.Body.PayPolicyInstallmentResult.Status.Info.FirstOrDefault().Description);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<UniqaGetPolicyInstallmentInfo> GetPolicyInstallmentInfo(string policyNo, string insuranceType, bool insuranceGroup)
        {
            var result = new UniqaGetPolicyInstallmentInfo();
            PSMPolicyServiceClient service = CreateConnection();


            var request = new getPolicyDueInstallmentRequest
            {
                AgentID = this._agentId,
                InsuranceGroup = insuranceGroup,
                InsuranceType = insuranceType,
                PolicyNumber = policyNo
            };

            string requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;

            GetPolicyDueInstallmentResponse1 installmentResult = await service.GetPolicyDueInstallmentAsync(request);

            if (installmentResult != null && installmentResult.Body != null)
            {
                if (installmentResult.Body.GetPolicyDueInstallmentResult != null)
                {
                    if (installmentResult.Body.GetPolicyDueInstallmentResult.Status.StatusType == 0)
                    {

                        var installment = installmentResult.Body.GetPolicyDueInstallmentResult.DueInstallment.FirstOrDefault();

                        if (installment != null)
                        {
                            result.AttachmentNumber = installment.AttachmentNumber;
                            result.InstallmentNumber = installment.InstallmentNumber;
                            result.InstallmentDate = installment.InstallmentDate;
                            result.Sum = installment.Sum;
                        }
                    }
                    else
                    {
                        string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                        int randomNumber = Helpers.GetRandomNumber();
                        string errorId = $"ins-{dateStr}-{randomNumber}";
                        //result.ErrorId = errorId;
                        string correlationId = installmentResult.Body.GetPolicyDueInstallmentResult.CorrelationID;
                        responseJson = JsonConvert.SerializeObject(installmentResult.Body);
                        //result.Message = policyResult.Body.IssueAlpinePolicyResult.Status.Info.FirstOrDefault().Description;

                        Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, errorId, correlationId); });
                        //throw new Exception(installmentResult.Body.PayPolicyInstallmentResult.Status.Info.FirstOrDefault().Description);
                        //this._logger.LogError($"Correlation Id : {installmentResult.Body.GetPolicyDueInstallmentResult.CorrelationID}");
                        //throw new Exception(installmentResult.Body.GetPolicyDueInstallmentResult.Status.Info.FirstOrDefault().Description);
                    }
                }

            }

            return result;
        }

        private PSMPolicyServiceClient CreateConnection()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            PSMPolicyServiceClient service = new PSMPolicyServiceClient();
            service.Endpoint.Address = new System.ServiceModel.EndpointAddress(_serviceUrl);
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindBySubjectName, _certName, false);

            X509Certificate2 cert = certificates[0];

            service.ClientCredentials.ClientCertificate.Certificate = cert;

            return service;
        }

        private bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            string certificateSubject = string.Empty;
            string certificateIssuer = string.Empty;
            X509ChainElementCollection chainElements;
            X509ChainPolicy chainPolicy;
            X509ChainStatus[] chainStatus;
            X509Certificate2 cert1;

            if (certificate != null)
            {
                certificateSubject = certificate.Subject;
                certificateIssuer = certificate.Issuer;
            }
            if (chain != null)
            {
                chainElements = chain.ChainElements;
                cert1 = chainElements[0].Certificate;
                //cert2 = chainElements[1].Certificate;
                chainPolicy = chain.ChainPolicy;
                chainStatus = chain.ChainStatus;
            }
            SslPolicyErrors sslPolicyErrors = policyErrors;

            return true;
        }

        private List<TravelAddRisk> AddRisk()
        {
            var risk = new List<TravelAddRisk>();
            risk.Add(new TravelAddRisk
            {
                RiskType = 95,
                Limit = 1000
            });

            risk.Add(new TravelAddRisk
            {
                RiskType = 97,
                Limit = 1000
            });


            risk.Add(new TravelAddRisk
            {
                RiskType = 96,
                Limit = 400
            });

            risk.Add(new TravelAddRisk
            {
                RiskType = 98,
                Limit = 1000
            });

            return risk;
        }

        private List<ClientData> GetClientData(List<InsuredClientDataModel> clients)
        {
            var clientData = new List<ClientData>();

            foreach (var client in clients)
            {
                var current = new ClientData
                {
                    PinType = (byte)IdentificationTypeEnum.UiNumber,
                    PIN = client.Uin,
                    FirstName = client.FirstName,
                    FamilyName = client.LastName,
                    BirthDate = client.BirthDate.Value
                };

                clientData.Add(current);
            }

            return clientData;

        }

        private List<PersonData> GetPersonData(List<InsuredClientDataModel> clients)
        {
            var clientData = new List<PersonData>();

            foreach (var client in clients)
            {
                var current = new PersonData
                {
                    PinType = (byte)IdentificationTypeEnum.UiNumber,
                    PIN = client.Uin,
                    Name = client.FirstName + " " + client.LastName,
                    BirthDate = client.BirthDate.Value,
                    Country = client.Country
                    
                };

                clientData.Add(current);
            }

            return clientData;

        }

        private Customer GetHolderData(InsurerCustomerModel holder)
        {
            byte type = 0;

            if (holder.Country != GlobalConstants.BgCountryCode)
            {
                type = (byte)PersonTypeEnum.Foreigner;
            }
            else
            {
                type = string.IsNullOrEmpty(holder.Uin) ? (byte)PersonTypeEnum.LegalEntity : (byte)PersonTypeEnum.Individual;
            }

            var customer = new Customer
            {
                Type = type,
                Name = holder.Name,
                PinType = string.IsNullOrEmpty(holder.Uin) ? (byte)IdentificationTypeEnum.VatNumber : (byte)IdentificationTypeEnum.UiNumber,
                PIN = string.IsNullOrEmpty(holder.Uin) ? holder.Vat : holder.Uin,
                CountryOriginal = holder.CountryOriginal,
                Country = GlobalConstants.BgCountryCode,
                Location = holder.Location,
                Address = holder.Address,
                BirthDate = holder.BirthDate,
                Age = (byte)holder.Age,
                Email = holder.Email,
                Phone = holder.Phone,
                InsuredType = GlobalConstants.UniqaInsuredType
            };

            return customer;
        }

        private List<UniqaSoapService.Question> GetQuestionnaire(QuestionnaireModel input)
        {
            var questions = new List<UniqaSoapService.Question>();

            foreach (var question in input.Questionnaire)
            {
                var current = new UniqaSoapService.Question
                {
                    QuestionID = (byte)question.QuestionId,
                    Answer = question.Answer
                };

                questions.Add(current);
            }

            return questions;
        }

        private List<MyThings> GetMyThings(int propertyTypeId, int objectTypeId, decimal insuranceSum, QuestionnaireModel questionnaire)
        {
            var result = new List<MyThings>();

            var questions = new List<UniqaSoapService.Question>();
            if (questionnaire != null)
            {
                questions = this.GetQuestionnaire(questionnaire);
            }

            var current = new MyThings
            {
                CoverID = GlobalConstants.UniqaMyThingsCoverId,
                PropertyTypeID = (byte)propertyTypeId,
                ObjectTypeID = (byte)objectTypeId,
                InsuranceSum = insuranceSum,
                Questionnaire = questions.ToArray()
            };

            result.Add(current);

            return result;
        }

        private void LogError(string exMessage, string trace)
        {
            string message = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm")} {trace} - {exMessage}";
            string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy")}.txt";

            this._errorService.LogError(message, fileName);
        }
        private async void LogInsuranceError(string requestJson, string responseJson, string idNumber, string correlationId)
        {
            string header = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm")} id:{idNumber}";
            string correlationIdField= $"CorrelationId - {correlationId}";
            string request = $"Request - {requestJson}";
            string response = $"Response - {responseJson}";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);
            sb.AppendLine(correlationIdField);
            sb.AppendLine(request);
            sb.AppendLine(response);

            string message = sb.ToString().TrimEnd();
            string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy")}.txt";

            this._errorService.LogError(message, fileName);
        }

        #endregion

    }
}
