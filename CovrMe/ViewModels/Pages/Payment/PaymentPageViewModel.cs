using CovrMe.Factories;
using CovrMe.Models.Deliveries;
using CovrMe.Models;
using CovrMe.Models.Deliveries.Result;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CovrMe.Models.Insurances.CivilInsurance;
using System.Timers;
using GreenDonut;
using CovrMe.Models.Insurances.CivilInsurance.Request;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Users;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Request.CivilInsurances;
using CovrMe.Models.Insurances.Request.TravelInsurance;
using CovrMe.Models.Insurances.Request.MountainInsurance;
using CovrMe.Models.Insurances.Request.HealthInsurance;
using CovrMe.Models.Insurances.Request.MyThings;

namespace CovrMe.ViewModels.Pages.Payment
{
    public partial class PaymentPageViewModel : BaseViewModel
    {
        #region Fields
       
        private decimal _totalPrice;
        private string _premiumFormatted;
        private string _policyNo;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;
        private InsuranceDocumentModel _insuranceDocuments;
        private List<InsuredUserDataModel> _insuredUsersInfo;

        private InsuranceDeliveryModel _deliveryInfo;
        private decimal _deliveryPrice = 0;

        //insurance
        private InsuranceOfferModel _selectedOffer;

        private string _source;
        private string _title;
        private string _localOrderId;
        private string _documentBatchId;
        private string _stickerId;
        private string _greencardId;

        private string _errorId;
        private string _errorMessage;

        private IPaymentService _paymentService;
        private IDeliveryService _deliveryService;
        private IInsuranceService _insuranceService;

        private static System.Timers.Timer _timer;

        #endregion

        public PaymentPageViewModel(IPaymentService paymentService, IDeliveryService deliveryService, IInsuranceService insuranceService)
        {
            _paymentService = paymentService;
            _deliveryService = deliveryService;
            Task.Run(async () => { await SetTimer(); });
            _insuranceService = insuranceService;

        }

        #region Props

        public string Source
        {
            get { return this._source; }
            set { SetProperty(ref this._source, value); }
        }

        public string Title
        {
            get { return this._title; }
            set { SetProperty(ref this._title, value); }
        }

        #endregion

        #region Methods

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._usualUserInfo = query.FirstOrDefault(x => x.Key == "usualUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
            this._insuranceDocuments = query.FirstOrDefault(x => x.Key == "insuranceDocuments").Value as InsuranceDocumentModel;
            this._insuredUsersInfo = query.FirstOrDefault(x => x.Key == "insuredUsers").Value as List<InsuredUserDataModel>;

            this._deliveryInfo = query.FirstOrDefault(x => x.Key == "deliveryInfo").Value as InsuranceDeliveryModel;

            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;

            this.Source = _insuranceDocuments.PaymentFormUrl;
            this.Title = _selectedOffer.Title;

        }

        private async Task SetTimer()
        {
            _timer = new System.Timers.Timer(5000);

            _timer.Elapsed += async (sender, e) => await CheckPaymentStatus();
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private async Task CheckPaymentStatus()
        {
            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var result = await this._paymentService.CheckPaymentStatus(this._insuranceDocuments.LocalOrderNumber, jwt, httpClient);

            if(result.Status == (int)GeneralStatusEnum.Success && (result.Operation == GlobalConstants.DskDepositedOperation ||
                result.Operation == GlobalConstants.DskApprovedOperation))
            {
                _timer.Stop();

                bool insuranceResult = false;

                switch (this._selectedOffer.InsuranceType)
                {
                    case (byte)InsuranceTypeEnum.Civil:

                        if (this._selectedOffer.NextInstallment)
                        {
                            insuranceResult = await this.PayCivilInstallment();
                        }
                        else
                        {
                            if (this._selectedOffer.LongSearch)
                            {
                                insuranceResult = await this.CreateLongCivilInsurance();
                            }
                            else
                            {
                                insuranceResult = await this.CreateCivilInsurance();
                            }
                            
                        }

                        if (insuranceResult)
                        {
                            var shipmentResult = await this.CreateSpeedyShipment();

                            //if (!string.IsNullOrEmpty(shipmentResult))
                            //{
                            //    await PerformNavigation((int)GeneralStatusEnum.Success);
                            //}
                            //else
                            //{
                            //    await PerformNavigation((int)GeneralStatusEnum.Unsuccess);
                            //}

                            await PerformNavigation((int)GeneralStatusEnum.Success);
                        }
                        else
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Unsuccess);
                        }

                    break;

                    case (byte)InsuranceTypeEnum.Travel:

                        insuranceResult = await this.CreateTravelInsurance();

                        if (insuranceResult)
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Success);
                        }
                        else
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Unsuccess);
                        }
                    break;

                    case (byte)InsuranceTypeEnum.Mountain:

                        insuranceResult = await this.CreateMountainInsurance();

                        if (insuranceResult)
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Success);
                        }
                        else
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Unsuccess);
                        }
                        break;
                    case (byte)InsuranceTypeEnum.Health:

                        
                        if (this._selectedOffer.NextInstallment)
                        {
                            insuranceResult = await this.PayHealthInstallment();
                        }
                        else
                        {
                            insuranceResult = await this.CreateHealthInsurance();
                        }

                        if (insuranceResult)
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Success);
                        }
                        else
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Unsuccess);
                        }
                        break;
                    case (byte)InsuranceTypeEnum.MyThings:

                        insuranceResult = await this.CreateMyThingsInsurance();

                        if (insuranceResult)
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Success);
                        }
                        else
                        {
                            await PerformNavigation((int)GeneralStatusEnum.Unsuccess);
                        }
                        break;
                }
            }
            else
            {
                if(result.Operation != GlobalConstants.DskAwaitingOperation)
                {
                    _timer.Stop();
                    await PerformNavigation((int)GeneralStatusEnum.Unsuccess);
                }
            }
        }

        private async Task<string> CreateSpeedyShipment()
        {
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            this._deliveryInfo.PolicyNo = this._policyNo;
            var shipment = await this._deliveryService.Shipment(this._deliveryInfo, jwt, httpClient);

            return shipment;
        }

        private async Task<bool> CreateCivilInsurance()
        {
            bool result = false;
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var input = new CivilInsurancePolicyInput
            {
                InsuranceCompany = this._selectedOffer.CompanyName,
                VehicleFirstReg = this._vehicleInfo.FirstRegistrationDate,
                VehicleBrand = this._vehicleInfo.VehicleBrand,
                VehicleModel = this._vehicleInfo.VehicleModel,
                VehicleModelId = this._vehicleInfo.VehicleModelId,
                VehiclePlateNumber = this._vehicleInfo.PlateNumber,
                VehicleUsage = this._vehicleInfo.VehicleUsage,
                VehicleType = this._vehicleInfo.VehicleTypeId,
                RegistrationCertificateNumber = this._vehicleInfo.RegistrationCertificateNumber,
                OwnerExperience = this._ownerUserInfo.DrivingExperiance,
                OwnerUiNumber = this._ownerUserInfo.Uin,
                OwnerVatNumber = this._ownerUserInfo.VatNumber,
                OwnerPostalCode = this._ownerUserInfo.PostalCode,
                OwnerCity = this._ownerUserInfo.CityName,
                OwnerRegion = this._ownerUserInfo.RegionName,
                OwnerMunicipality = this._ownerUserInfo.MunicipalityName,
                OwnerCountryCode = this._ownerUserInfo.CountryCode,
                OwnerName = string.IsNullOrEmpty(this._ownerUserInfo.VatNumber) ? (this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.SurName + " " + this._ownerUserInfo.LastName) : this._ownerUserInfo.CompanyName,
                OwnerAddress = this._ownerUserInfo.Address,
                Installments = this._selectedOffer.Installment,
                OwnerBirthdate = this._ownerUserInfo.BirthDateString,
                UsualExperience = this._usualUserInfo != null ? _usualUserInfo.DrivingExperiance : 0,
                UsualUiNumber = this._usualUserInfo != null ? _usualUserInfo.Uin : string.Empty,
                UsualPostalCode = this._usualUserInfo != null ? this._usualUserInfo.PostalCode : string.Empty,
                UsualRegion = this._usualUserInfo != null ? this._usualUserInfo.RegionName : string.Empty,
                UsualMunicipality = this._usualUserInfo != null ? this._usualUserInfo.MunicipalityName : string.Empty,
                UsualName = this._usualUserInfo != null ? this._usualUserInfo.FirstName + " " + this._usualUserInfo.SurName + " " + this._usualUserInfo.LastName : string.Empty,
                UsualAddress = this._usualUserInfo != null ? this._usualUserInfo.Address : string.Empty,
                UsualBirthdate = this._usualUserInfo != null ? this._usualUserInfo.BirthDateString : string.Empty,
                UsualCountryCode = this._usualUserInfo != null ? this._usualUserInfo.CountryCode : string.Empty,
                UsualUserId = this._usualUserInfo != null ? this._usualUserInfo.UserId : string.Empty,
                StartDate = App.CalendarStartDate,
                GreenCardId = this._insuranceDocuments.GreenCardId,
                StickerId = this._insuranceDocuments.StickerId,
                LocalOrderNumber = this._insuranceDocuments.LocalOrderNumber,
                Email = this._selectedOffer.Email,
                UserId = this._ownerUserInfo.UserId,
                MainUserId = App.UserId,
                Phone = this._deliveryInfo.Phone,
                UsualCity = this._usualUserInfo != null ? this._usualUserInfo.CityName : string.Empty,
            };

            var policy = await this._insuranceService.CivilInsurancePolicy(input, httpClient, jwt);

            if(policy != null && !string.IsNullOrEmpty(policy.ErrorId))
            {
                this._errorId = policy.ErrorId;
                this._errorMessage = policy.Message;
            }
            else if(policy != null && policy.Success == (int)GeneralStatusEnum.Success)
            {                
                this._policyNo = policy.PolicyNo;
                result = true;              
            }

            return result;
        }

        private async Task<bool> CreateLongCivilInsurance()
        {
            bool result = false;
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var input = new CivilInsuranceLongPolicyInput
            {
                InsuranceCompany = this._selectedOffer.CompanyName,
                VehicleFirstReg = this._vehicleInfo.FirstRegistrationDate,
                VehicleBrand = this._vehicleInfo.VehicleBrand,
                VehicleModel = this._vehicleInfo.VehicleModel,
                VehicleModelId = this._vehicleInfo.VehicleModelId,
                VehiclePlateNumber = this._vehicleInfo.PlateNumber,
                VehicleUsage = this._vehicleInfo.VehicleUsage,
                VehicleType = this._vehicleInfo.VehicleTypeId,
                RegistrationCertificateNumber = this._vehicleInfo.RegistrationCertificateNumber,
                OwnerExperience = this._ownerUserInfo.DrivingExperiance,
                OwnerUiNumber = this._ownerUserInfo.Uin,
                OwnerVatNumber = this._ownerUserInfo.VatNumber,
                OwnerPostalCode = this._ownerUserInfo.PostalCode,
                OwnerCity = this._ownerUserInfo.CityName,
                OwnerRegion = this._ownerUserInfo.RegionName,
                OwnerMunicipality = this._ownerUserInfo.MunicipalityName,
                OwnerCountryCode = this._ownerUserInfo.CountryCode,
                OwnerName = string.IsNullOrEmpty(this._ownerUserInfo.VatNumber) ? (this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.SurName + " " + this._ownerUserInfo.LastName) : this._ownerUserInfo.CompanyName,
                OwnerAddress = this._ownerUserInfo.Address,
                Installments = this._selectedOffer.Installment,
                OwnerBirthdate = this._ownerUserInfo.BirthDateString,
                UsualExperience = this._usualUserInfo != null ? _usualUserInfo.DrivingExperiance : 0,
                UsualUiNumber = this._usualUserInfo != null ? _usualUserInfo.Uin : string.Empty,
                UsualPostalCode = this._usualUserInfo != null ? this._usualUserInfo.PostalCode : string.Empty,
                UsualRegion = this._usualUserInfo != null ? this._usualUserInfo.RegionName : string.Empty,
                UsualMunicipality = this._usualUserInfo != null ? this._usualUserInfo.MunicipalityName : string.Empty,
                UsualName = this._usualUserInfo != null ? this._usualUserInfo.FirstName + " " + this._usualUserInfo.SurName + " " + this._usualUserInfo.LastName : string.Empty,
                UsualAddress = this._usualUserInfo != null ? this._usualUserInfo.Address : string.Empty,
                UsualBirthdate = this._usualUserInfo != null ? this._usualUserInfo.BirthDateString : string.Empty,
                UsualCountryCode = this._usualUserInfo != null ? this._usualUserInfo.CountryCode : string.Empty,
                UsualUserId = this._usualUserInfo != null ? this._usualUserInfo.UserId : string.Empty,
                StartDate = App.CalendarStartDate,
                GreenCardId = this._insuranceDocuments.GreenCardId,
                StickerId = this._insuranceDocuments.StickerId,
                LocalOrderNumber = this._insuranceDocuments.LocalOrderNumber,
                Email = this._selectedOffer.Email,
                UserId = this._ownerUserInfo.UserId,
                MainUserId = App.UserId,
                Phone = this._deliveryInfo.Phone,
                OwnerGuilt = this._ownerUserInfo.GuiltTypeId,
                UsualGuilt = this._usualUserInfo != null ? this._usualUserInfo.GuiltTypeId : 0,
                Vin = this._vehicleInfo.Vin,
                VehicleEngineType = this._vehicleInfo.EngineTypeText,
                VehicleBodyType = this._vehicleInfo.BodyTypeText,
                VehicleEngineVolume = this._vehicleInfo.EngineVolume,
                VehiclePlaces = this._vehicleInfo.Places,
                VehicleColor = this._vehicleInfo.ColorText,
                VehicleSteeringWheel = this._vehicleInfo.SteeringWheel,
                VehicleKilowatts = this._vehicleInfo.VehicleKilowatts,
                UsualCity = this._usualUserInfo != null ? this._usualUserInfo.CityName : string.Empty,
            };

            var policy = await this._insuranceService.CivilInsuranceLongPolicy(input, httpClient, jwt);

            if (policy != null && !string.IsNullOrEmpty(policy.ErrorId))
            {
                this._errorId = policy.ErrorId;
                this._errorMessage = policy.Message;
            }
            else if (policy != null && policy.Success == (int)GeneralStatusEnum.Success)
            {
                this._policyNo = policy.PolicyNo;
                result = true;
            }

            return result;
        }

        private async Task<bool> CreateTravelInsurance()
        {
            bool result = false;
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var input = new TravelPolicyInput
            {
                InsuranceCompany = this._selectedOffer.CompanyName,
                PolicyType = this._selectedOffer.TravelInsuranceInfo.PolicyType,
                StartDate = this._selectedOffer.StartDate,
                EndDate = this._selectedOffer.EndDate,
                TripPurpose = this._selectedOffer.TravelInsuranceInfo.TripPurpose,
                Territory = this._selectedOffer.TravelInsuranceInfo.Territory,
                Limit = this._selectedOffer.TravelInsuranceInfo.Limit,
                Username = this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.LastName,
                LocalOrderNumber = this._insuranceDocuments.LocalOrderNumber,
                MainUserId = App.UserId,
                Email = this._selectedOffer.Email,
                Clients = await this.AddInsuredUsers(),
                Holder = new InsurerCustomerModel
                {
                    UserId = this._ownerUserInfo.UserId,
                    Name = string.IsNullOrEmpty(this._ownerUserInfo.VatNumber) ? (this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.SurName + " " + this._ownerUserInfo.LastName) : this._ownerUserInfo.CompanyName,
                    Uin = this._ownerUserInfo.Uin,
                    Vat = this._ownerUserInfo.VatNumber,
                    Country = this._ownerUserInfo.CountryCode,
                    CountryOriginal = this._ownerUserInfo.CountryCode,
                    Location = this._ownerUserInfo.CityCode,
                    Address = this._ownerUserInfo.Address,
                    BirthDate = this._ownerUserInfo.BirthDate,
                    Age = this._ownerUserInfo.Age,
                    Email = this._ownerUserInfo.Email,
                    Phone = this._ownerUserInfo.Phone
                }
            };

            var policy = await this._insuranceService.TravelPolicy(input, jwt,httpClient);

            if (policy != null)
            {
                if (!string.IsNullOrEmpty(policy.ErrorCode))
                {
                    this._errorId = policy.ErrorCode;
                    this._errorMessage = policy.Message;
                }
                else if (!string.IsNullOrEmpty(policy.PolicyNumber))
                {
                    this._policyNo = policy.PolicyNumber;
                    result = true;
                }               
            }

            return result;
        }

        private async Task<bool> CreateMountainInsurance()
        {
            bool result = false;
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var input = new MountainPolicyInput
            {
                InsuranceCompany = this._selectedOffer.CompanyName,
                StartDate = this._selectedOffer.StartDate,
                EndDate = this._selectedOffer.EndDate,
                InsuranceSum = this._selectedOffer.MountainInsuranceInfo.InsuranceSum,
                Username = this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.LastName,
                IsExtreme = this._selectedOffer.MountainInsuranceInfo.IsExtreme,
                LocalOrderNumber = this._insuranceDocuments.LocalOrderNumber,
                MainUserId = App.UserId,
                Email = this._selectedOffer.Email,
                Clients = await this.AddInsuredUsers(),
                Holder = new InsurerCustomerModel
                {
                    UserId = this._ownerUserInfo.UserId,
                    Name = string.IsNullOrEmpty(this._ownerUserInfo.VatNumber) ? (this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.SurName + " " + this._ownerUserInfo.LastName) : this._ownerUserInfo.CompanyName,
                    Uin = this._ownerUserInfo.Uin,
                    Vat = this._ownerUserInfo.VatNumber,
                    Country = this._ownerUserInfo.CountryCode,
                    CountryOriginal = this._ownerUserInfo.CountryCode,
                    Location = this._ownerUserInfo.CityCode,
                    Address = this._ownerUserInfo.Address,
                    BirthDate = this._ownerUserInfo.BirthDate,
                    Age = this._ownerUserInfo.Age,
                    Email = this._ownerUserInfo.Email,
                    Phone = this._ownerUserInfo.Phone
                }
            };

            var policy = await this._insuranceService.MountainPolicy(input, jwt, httpClient);

            if (!string.IsNullOrEmpty(policy.ErrorCode))
            {
                this._errorId = policy.ErrorCode;
                this._errorMessage = policy.Message;
            }
            else if (!string.IsNullOrEmpty(policy.PolicyNumber))
            {
                this._policyNo = policy.PolicyNumber;
                result = true;
            }

            return result;
        }

        private async Task<bool> CreateHealthInsurance()
        {
            bool result = false;
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var input = new HealthPolicyInput
            {
                InsuranceCompany = this._selectedOffer.CompanyName,
                StartDate = this._selectedOffer.StartDate,
                EndDate = this._selectedOffer.EndDate,
                PacketId = this._selectedOffer.HealthInsuranceInfo.PacketId,
                IsFamily = this._selectedOffer.HealthInsuranceInfo.IsFamily,
                InstallmentCount = this._selectedOffer.Installment,
                Username = this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.LastName,
                LocalOrderNumber = this._insuranceDocuments.LocalOrderNumber,
                MainUserId = App.UserId,
                Questionnaire = this._selectedOffer.HealthInsuranceInfo.Questionnaire,
                Email = this._selectedOffer.Email,
                Clients = await this.AddInsuredUsers(),
                Holder = new InsurerCustomerModel
                {
                    UserId = this._ownerUserInfo.UserId,
                    Name = string.IsNullOrEmpty(this._ownerUserInfo.VatNumber) ? (this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.SurName + " " + this._ownerUserInfo.LastName) : this._ownerUserInfo.CompanyName,
                    Uin = this._ownerUserInfo.Uin,
                    Vat = this._ownerUserInfo.VatNumber,
                    Country = this._ownerUserInfo.CountryCode,
                    CountryOriginal = this._ownerUserInfo.CountryCode,
                    Location = this._ownerUserInfo.CityCode,
                    Address = this._ownerUserInfo.Address,
                    BirthDate = this._ownerUserInfo.BirthDate,
                    Age = this._ownerUserInfo.Age,
                    Email = this._ownerUserInfo.Email,
                    Phone = this._ownerUserInfo.Phone
                }
            };

            var policy = await this._insuranceService.HealthPolicy(input, jwt, httpClient);

            if (!string.IsNullOrEmpty(policy.ErrorCode))
            {
                this._errorId = policy.ErrorCode;
                this._errorMessage = policy.Message;
            }
            else if (!string.IsNullOrEmpty(policy.PolicyNumber))
            {
                this._policyNo = policy.PolicyNumber;
                result = true;
            }

            return result;
        }

        private async Task<bool> PayHealthInstallment()
        {
            bool result = false;
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var input = new HealthInsuranceInstallmentInput
            {
                InsuranceId = this._selectedOffer.InsuranceId,
                UserId = this._ownerUserInfo.UserId,
                LocalOrderNumber = this._insuranceDocuments.LocalOrderNumber,
                Email = this._selectedOffer.Email
            };

            var installmentResult = await this._insuranceService.HealthInsuranceInstallment(input, jwt, httpClient);
            this._policyNo = this._selectedOffer.PolicyNo;

            if (installmentResult != null && installmentResult.Code == (int)GeneralStatusEnum.Success)
            {
                result = true;
            }
            else
            {
                this._errorId = installmentResult.ErrorId;
                this._errorMessage = installmentResult.Message;
                result = false;
            }

            return result;
        }

        private async Task<bool> CreateMyThingsInsurance()
        {
            bool result = false;
            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var input = new MyThingsPolicyInput
            {
                InsuranceCompany = this._selectedOffer.CompanyName,
                StartDate = this._selectedOffer.StartDate,
                EndDate = this._selectedOffer.EndDate,
                InsuranceSum = this._selectedOffer.MyThingsInsuranceInfo.InsuranceSum,
                Username = this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.LastName,
                LocalOrderNumber = this._insuranceDocuments.LocalOrderNumber,
                MainUserId = App.UserId,
                PropertyTypeId = this._selectedOffer.MyThingsInsuranceInfo.PropertyTypeId,
                ObjectTypeId = this._selectedOffer.MyThingsInsuranceInfo.ObjectTypeId,
                Questionnaire = this._selectedOffer.MyThingsInsuranceInfo.Questionnaire,
                Brand = this._selectedOffer.MyThingsInsuranceInfo.Brand,
                Model = this._selectedOffer.MyThingsInsuranceInfo.Model,
                Email = this._selectedOffer.Email,
                Holder = new InsurerCustomerModel
                {
                    UserId = this._ownerUserInfo.UserId,
                    Name = string.IsNullOrEmpty(this._ownerUserInfo.VatNumber) ? (this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.SurName + " " + this._ownerUserInfo.LastName) : this._ownerUserInfo.CompanyName,
                    Uin = this._ownerUserInfo.Uin,
                    Vat = this._ownerUserInfo.VatNumber,
                    Country = this._ownerUserInfo.CountryCode,
                    CountryOriginal = this._ownerUserInfo.CountryCode,
                    Location = this._ownerUserInfo.CityCode,
                    Address = this._ownerUserInfo.Address,
                    BirthDate = this._ownerUserInfo.BirthDate,
                    Age = this._ownerUserInfo.Age,
                    Email = this._ownerUserInfo.Email,
                    Phone = this._ownerUserInfo.Phone
                }
            };

            var policy = await this._insuranceService.MyThingsPolicy(input, jwt, httpClient);

            if (!string.IsNullOrEmpty(policy.ErrorCode))
            {
                this._errorId = policy.ErrorCode;
                this._errorMessage = policy.Message;
            }
            else if (!string.IsNullOrEmpty(policy.PolicyNumber))
            {
                this._policyNo = policy.PolicyNumber;
                result = true;
            }

            return result;
        }

        private async Task<bool> PayCivilInstallment()
        {
            bool result = false;

            var httpClient = HttpClientFactory.Create();
            string jwt = App.JwtToken;

            var req = new CivilInsuranceInstallmentInput
            {
                InsuranceId = this._selectedOffer.InsuranceId,
                UserId = this._ownerUserInfo.UserId,
                GreenCardId = this._insuranceDocuments.GreenCardId,
                StickerId = this._insuranceDocuments.StickerId,
                LocalOrderNumber = this._insuranceDocuments.LocalOrderNumber,
                Email = this._selectedOffer.Email
            };

            var installmentResult = await this._insuranceService.CivilInsuranceInstallmentPay(req, jwt, httpClient);
            this._policyNo = this._selectedOffer.PolicyNo;

            if(installmentResult != null && installmentResult.Code == (int)GeneralStatusEnum.Success)
            {
                result = true;
            }
            else
            {
                this._errorId = installmentResult.ErrorId;
                this._errorMessage = installmentResult.Message;
                result = false;
            }

            return result;
        }

        private async Task PerformNavigation(int success)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    if (success == (int)GeneralStatusEnum.Success)
                    {
                        await Navigation.PushAsync<ThankYouPage>();
                    }
                    else
                    {
                        var parameters = new Dictionary<string, object>
                        {
                            {"errorId", this._errorId},
                            {"errorMessage", this._errorMessage},
                        };

                        await Navigation.PushAsync<ErrorPage>(parameters);
                    }
                  
                }
                finally
                {
                    cancellationTokenSource.Cancel();
                }
            });

        }
       
        private async Task<List<InsuredClientDataModel>> AddInsuredUsers()
        {
            var result = new List<InsuredClientDataModel>();

            if(this._insuredUsersInfo != null)
            {
                foreach (var user in this._insuredUsersInfo)
                {
                    var current = new InsuredClientDataModel
                    {
                        UserId = user.SelectedUser != null ? user.SelectedUser.Id : null,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Uin = user.Uin,
                        BirthDate = user.BirthDate
                    };

                    result.Add(current);
                }
            }

            return result;
        }

        #endregion
    }
}
