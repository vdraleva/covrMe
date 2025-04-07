using CovrMe.WebAPI.Models.Request.Sirma;
using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Services.Sirma.Contracts;
using CovrMe.WebAPI.Models.Request.Sirma;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Models.Result.InsuranceCompanies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Shared.Helpers;
using CovrMe.WebAPI.Models.Result.Currencies;
using CovrMe.WebAPI.Models.Result.Payment;
using Microsoft.EntityFrameworkCore;
using CovrMe.WebAPI.Data.Context;
using CovrMe.WebAPI.Models.Result.Vehicles;
using Microsoft.AspNetCore.Localization;
using UniqaSoapService;
using Org.BouncyCastle.Bcpg.Sig;
using Org.BouncyCastle.Asn1.Utilities;
using CovrMe.WebAPI.Services.Messaging;
using CovrMe.WebAPI.Models;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Request.Insurances.CivilInsurance;
using CovrMe.WebAPI.Models.Request.Insurances;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.CivilInsurance;
using CovrMe.WebAPI.Models.Request.Insurances.Travel;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Travel;
using CovrMe.WebAPI.Services.Uniqa.Contracts;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using CovrMe.WebAPI.Models.Result.Insurances.Travel;
using CovrMe.WebAPI.Models.Result.Insurances.Mountain;
using CovrMe.WebAPI.Models.Request.Insurances.Mountain;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Mountain;
using CovrMe.WebAPI.Models.Request.Insurances.Health;
using CovrMe.WebAPI.Models.Result.Insurances.Health;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.Health;
using CovrMe.WebAPI.Models.Result.Insurances.MyThings;
using CovrMe.WebAPI.Models.Request.Insurances.MyThings;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads.MyThings;
using CovrMe.WebAPI.Models.Request.Insurances.Casco;
using CovrMe.WebAPI.Models.Result.Users.Payloads;
using CovrMe.WebAPI.Models.Result.Insurances.Casco;
using Newtonsoft.Json;
using System.Text;
using CovrMe.WebAPI.Models.Result.Insurances.Payloads;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class InsuranceService : IInsuranceService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private ISirmaCivilInsuranceService _civilInsuranceService;
        private IInsuranceCompanyService _insuranceCompanyService;
        private IPaymentService _paymentService;
        private ICurrencyService _currencyService;
        private IVehicleService _vehicleService;
        private IRepository<Insurance> _insuranceRepository;
        private IRepository<CivilInsurance> _civilInsurancesRepository;
        private IRepository<UsersInsurance> _usersInsurancesRepository;
        private IRepository<InsurancesPaymentInformation> _insurancePaymentInfoRepository;
        private readonly ApplicationDbContext _dbContext;
        private IDocumentService _documentService;
        private IReadWriteService _readWriteService;
        private IMailService _mailService;
        private IErrorService _errorService;
        private IUniqaInsuranceService _uniqaService;
        private IRepository<TravelInsurance> _travelInsurancesRepository;
        private IRepository<MountainInsurance> _mountainInsurancesRepository;
        private IRepository<HealthInsurance> _healthInsurancesRepository;
        private IRepository<MyThingsInsurance> _myThingsInsurancesRepository;
        private IUserService _userService;
        private IQuestionService _questionService;
        private IFtpService _ftpService;

        public InsuranceService(ILogger<InsuranceService> logger, IConfiguration configuration, ISirmaCivilInsuranceService civilInsuranceService,
            IInsuranceCompanyService insuranceCompanyService, IRepository<CivilInsurance> civilInsurancesRepository,
            IRepository<Insurance> insuranceRepository, IRepository<UsersInsurance> usersInsurancesRepository, ICurrencyService currencyService,
            IPaymentService paymentService, ApplicationDbContext dbContext, IVehicleService vehicleService, IDocumentService documentService,
            IRepository<InsurancesPaymentInformation> insurancePaymentInfoRepository, IReadWriteService readWriteService,
            IMailService mailService, IErrorService errorService, IUniqaInsuranceService uniqaService, IRepository<TravelInsurance> travelInsurancesRepository,
            IUserService userService, IRepository<MountainInsurance> mountainInsurancesRepository, IRepository<HealthInsurance> healthInsurancesRepository,
            IRepository<MyThingsInsurance> myThingsInsurancesRepository, IQuestionService questionService, IFtpService ftpService)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._civilInsuranceService = civilInsuranceService;
            this._insuranceCompanyService = insuranceCompanyService;
            this._civilInsurancesRepository = civilInsurancesRepository;
            this._insuranceRepository = insuranceRepository;
            this._usersInsurancesRepository = usersInsurancesRepository;
            this._currencyService = currencyService;
            this._paymentService = paymentService;
            this._dbContext = dbContext;
            this._vehicleService = vehicleService;
            this._mailService = mailService;
            this._userService = userService;
            this._mountainInsurancesRepository = mountainInsurancesRepository;

            this._documentService = documentService;
            this._insurancePaymentInfoRepository = insurancePaymentInfoRepository;
            this._readWriteService = readWriteService;
            this._errorService = errorService;
            this._uniqaService = uniqaService;
            this._travelInsurancesRepository = travelInsurancesRepository;
            this._healthInsurancesRepository = healthInsurancesRepository;
            this._myThingsInsurancesRepository = myThingsInsurancesRepository;
            this._questionService = questionService;
            this._ftpService = ftpService;
        }

        #region Civil Insurance
        public async Task<CivilInsuranceSearchPayload> CivilInsuranceSearch(CivilInsuranceSearchInput req)
        {
            var result = new CivilInsuranceSearchPayload();
            var sirmaRequest = new CivilInsuranceSearchRequestModel();

            if (!string.IsNullOrEmpty(req.OwnerVatNumber))
            {
                sirmaRequest.OwnerPersonType = 1;
                sirmaRequest.UiNumber = req.OwnerVatNumber;

                sirmaRequest.HasUsualDriver = 1;
                sirmaRequest.UsualExperience = req.UsualExperience;
                sirmaRequest.UiNumberUsual = req.UsualUiNumber;
                sirmaRequest.PostalCodeUsual = req.UsualPostalCode;
                sirmaRequest.UsualPersonType = 2;
                sirmaRequest.RegionUsual = req.UsualRegion;
                sirmaRequest.МunicipalityUsual = req.UsualMunicipality;
                sirmaRequest.NameUsual = req.UsualName;
                sirmaRequest.AddressUsual = req.UsualAddress;
                sirmaRequest.BirthdateUsual = req.UsualBirthdate;
                sirmaRequest.UsualCountryCode = req.UsualCountryCode;
                sirmaRequest.UsualCity = req.UsualCity;

                if (req.UsualCountryCode == GlobalConstants.BgCountryCode)
                {
                    sirmaRequest.NationalityUsual = 1;
                }
                else
                {
                    sirmaRequest.NationalityUsual = 0;
                }
            }
            else
            {
                sirmaRequest.OwnerPersonType = 2;
                sirmaRequest.UiNumber = req.OwnerUiNumber;
            }

            if (req.OwnerCountryCode == GlobalConstants.BgCountryCode)
            {
                sirmaRequest.Nationality = 1;
            }
            else
            {
                sirmaRequest.Nationality = 0;
            }

            sirmaRequest.OwnerExperience = req.OwnerExperience;
            sirmaRequest.VehiclePlateNumber = req.VehiclePlateNumber;
            sirmaRequest.VehicleUsage = req.VehicleUsage;
            sirmaRequest.RegistrationCertificateNumber = req.RegistrationCertificateNumber;
            sirmaRequest.PostalCode = req.OwnerPostalCode;
            sirmaRequest.VehicleType = req.VehicleType;
            sirmaRequest.Office = "0";
            sirmaRequest.City = req.OwnerCity;
            sirmaRequest.Region = req.OwnerRegion;
            sirmaRequest.Мunicipality = req.OwnerMunicipality;
            sirmaRequest.Name = req.OwnerName;
            sirmaRequest.Address = req.OwnerAddress;
            sirmaRequest.Birthdate = req.OwnerBirthdate;
            sirmaRequest.VehicleFirstReg = req.VehicleFirstReg;
            sirmaRequest.VehicleBrand = req.VehicleBrand;
            sirmaRequest.VehicleModel = req.VehicleModel;
            sirmaRequest.OwnerCountryCode = req.OwnerCountryCode;

            var insuranceCompanies = await this._insuranceCompanyService.GetCompanyNames();

            sirmaRequest.Insurers = string.Join(',', insuranceCompanies).ToLower();

            string requestJson = JsonConvert.SerializeObject(req);
            string responseJson = string.Empty;
            bool isError = false;
            string errorMessage = string.Empty;

            try
            {
                var civilInsurancesResponse = await this._civilInsuranceService.CivilInsuranceSearch(sirmaRequest, req.Installments);

                if (civilInsurancesResponse != null && civilInsurancesResponse.Success == (int)GeneralStatusEnum.Success)
                {
                    if (civilInsurancesResponse.CivilInsurances != null)
                    {
                        responseJson = JsonConvert.SerializeObject(civilInsurancesResponse);
                        if (civilInsurancesResponse.CivilInsurances.DziCivilInsurance != null)
                        {

                            if(!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.DziCivilInsurance.Error) ||
                                !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.DziCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.DziCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.DziCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var dziInsurance = civilInsurancesResponse.CivilInsurances.DziCivilInsurance;
                            dziInsurance.InsuranceCompanyName = GlobalConstants.Dzi;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Dzi);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(dziInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance.Error) ||
                                    !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance.Error;
                                }

                                isError = true;
                            }

                            var euroinsInsurance = civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance;
                            euroinsInsurance.InsuranceCompanyName = GlobalConstants.Euroins;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Euroins);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(euroinsInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance.Error) ||
                                    !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance.Error;
                                }

                                isError = true;
                            }

                            var generaliInsurance = civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance;
                            generaliInsurance.InsuranceCompanyName = GlobalConstants.Generali;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Generali);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(generaliInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance.Error) ||
                                    !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance.Error;
                                }

                                isError = true;
                            }

                            var bulinsInsurance = civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance;
                            bulinsInsurance.InsuranceCompanyName = GlobalConstants.Bulins;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Bulins);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(bulinsInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance.Error) ||
                                    !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance.Error;
                                }

                                isError = true;
                            }

                            var bulstradInsurance = civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance;
                            bulstradInsurance.InsuranceCompanyName = GlobalConstants.Bulstrad;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Bulstrad);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(bulstradInsurance);
                            }

                        }

                        if (civilInsurancesResponse.CivilInsurances.OzkCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.OzkCivilInsurance.Error) ||
                                    !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.OzkCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.OzkCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.OzkCivilInsurance.Error;
                                }

                                isError = true;
                            }

                            var ozkInsurance = civilInsurancesResponse.CivilInsurances.OzkCivilInsurance;
                            ozkInsurance.InsuranceCompanyName = GlobalConstants.Ozk;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Ozk);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(ozkInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance.Error) ||
                                    !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance.Error;
                                }

                                isError = true;
                            }

                            var groupamaInsurance = civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance;
                            groupamaInsurance.InsuranceCompanyName = GlobalConstants.Groupama;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Groupama);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(groupamaInsurance);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(civilInsurancesResponse.Error))
                {
                    if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.Error))
                    {
                        errorMessage = civilInsurancesResponse.Error;
                    }
                    isError = true;
                }

                if (isError)
                {
                    string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                    int randomNumber = Helpers.GetRandomNumber();
                    string id = $"ins-{dateStr}-{randomNumber}";
                    result.ErrorId = id;
                    result.Message = errorMessage;

                    Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, id); });
                }

                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                return result;
            }
        }

        public async Task<CivilInsuranceLongSearchPayload> CivilInsuranceLongSearch(CivilInsuranceLongSearchInput req)
        {
            var result = new CivilInsuranceLongSearchPayload();
            var sirmaRequest = new CivilInsuranceLongSearchRequestModel();

            if (!string.IsNullOrEmpty(req.OwnerVatNumber))
            {
                sirmaRequest.OwnerPersonType = 1;
                sirmaRequest.UiNumber = req.OwnerVatNumber;

                sirmaRequest.HasUsualDriver = 1;
                sirmaRequest.UsualExperience = req.UsualExperience;
                sirmaRequest.UiNumberUsual = req.UsualUiNumber;
                sirmaRequest.PostalCodeUsual = req.UsualPostalCode;
                sirmaRequest.UsualPersonType = 2;
                sirmaRequest.RegionUsual = req.UsualRegion;
                sirmaRequest.МunicipalityUsual = req.UsualMunicipality;
                sirmaRequest.NameUsual = req.UsualName;
                sirmaRequest.AddressUsual = req.UsualAddress;
                sirmaRequest.BirthdateUsual = req.UsualBirthdate;
                sirmaRequest.UsualCountryCode = req.UsualCountryCode;
                sirmaRequest.UsualGuilt = req.UsualGuilt;
                sirmaRequest.UsualCity = req.UsualCity;

                if (req.UsualCountryCode == GlobalConstants.BgCountryCode)
                {
                    sirmaRequest.NationalityUsual = 1;
                }
                else
                {
                    sirmaRequest.NationalityUsual = 0;
                }
            }
            else
            {
                sirmaRequest.OwnerPersonType = 2;
                sirmaRequest.UiNumber = req.OwnerUiNumber;
            }

            if (req.OwnerCountryCode == GlobalConstants.BgCountryCode)
            {
                sirmaRequest.Nationality = 1;
            }
            else
            {
                sirmaRequest.Nationality = 0;
            }

            sirmaRequest.OwnerExperience = req.OwnerExperience;
            sirmaRequest.VehiclePlateNumber = req.VehiclePlateNumber;
            sirmaRequest.VehicleUsage = req.VehicleUsage;
            sirmaRequest.RegistrationCertificateNumber = req.RegistrationCertificateNumber;
            sirmaRequest.PostalCode = req.OwnerPostalCode;
            sirmaRequest.VehicleType = req.VehicleType;
            sirmaRequest.Office = "0";
            sirmaRequest.City = req.OwnerCity;
            sirmaRequest.Region = req.OwnerRegion;
            sirmaRequest.Мunicipality = req.OwnerMunicipality;
            sirmaRequest.Name = req.OwnerName;
            sirmaRequest.Address = req.OwnerAddress;
            sirmaRequest.Birthdate = req.OwnerBirthdate;
            sirmaRequest.VehicleFirstReg = req.VehicleFirstReg;
            sirmaRequest.VehicleBrand = req.VehicleBrand;
            sirmaRequest.VehicleModel = req.VehicleModel;
            sirmaRequest.OwnerCountryCode = req.OwnerCountryCode;
            sirmaRequest.OwnerGuilt = req.OwnerGuilt;
            sirmaRequest.VehicleVin = req.Vin;
            sirmaRequest.EngineVolume = req.VehicleEngineVolume;
            sirmaRequest.VehicleBodyType = req.VehicleBodyType;
            sirmaRequest.VehiclePlaces = req.VehiclePlaces;
            sirmaRequest.VehicleColor = req.VehicleColor;
            sirmaRequest.VehicleEngineType = req.VehicleEngineType;
            sirmaRequest.VehicleSteeringWheel = req.VehicleSteeringWheel;
            sirmaRequest.VehicleKilowatts = req.VehicleKilowatts;
            //sirmaRequest.Insurers = insuranceCompanies.Where(x => x == "dzi" || x == "levins").ToList();

            var allInsuranceCompanies = await this._insuranceCompanyService.GetCompanyNames();

            if (req.InsuranceCompanies == null)
            {
                sirmaRequest.Insurers = allInsuranceCompanies.Where(x => x != "uniqa").ToList();
            }
            else
            {
                var excludedInsuranceCompanies = req.InsuranceCompanies;

                sirmaRequest.Insurers = allInsuranceCompanies
                    .Where(company => !excludedInsuranceCompanies.Contains(company) && company != "uniqa")
                    .ToList();

            }

            sirmaRequest.NetWeight = req.NetWeight;
            sirmaRequest.GrossWeight = req.GrossWeight;
            //sirmaRequest.BirthdateUsual = req.UsualBirthdate;

            string requestJson = JsonConvert.SerializeObject(req);
            string responseJson = string.Empty;
            bool isError = false;
            string errorMessage = string.Empty;

            try
            {
                var civilInsurancesResponse = await this._civilInsuranceService.CivilInsuranceLongSearch(sirmaRequest, req.Installments);

                if (civilInsurancesResponse != null && civilInsurancesResponse.Success == (int)GeneralStatusEnum.Success)
                {
                    if (civilInsurancesResponse.CivilInsurances != null)
                    {
                        responseJson = JsonConvert.SerializeObject(civilInsurancesResponse);
                        if (civilInsurancesResponse.CivilInsurances.DziCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.DziCivilInsurance.Error) ||
                               !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.DziCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.DziCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.DziCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var dziInsurance = civilInsurancesResponse.CivilInsurances.DziCivilInsurance;
                            dziInsurance.InsuranceCompanyName = GlobalConstants.Dzi;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Dzi);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(dziInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance.Error) ||
                                   !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var euroinsInsurance = civilInsurancesResponse.CivilInsurances.EuroinsCivilInsurance;
                            euroinsInsurance.InsuranceCompanyName = GlobalConstants.Euroins;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Euroins);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(euroinsInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance.Error) ||
                                   !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var generaliInsurance = civilInsurancesResponse.CivilInsurances.GeneraliCivilInsurance;
                            generaliInsurance.InsuranceCompanyName = GlobalConstants.Generali;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Generali);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(generaliInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance.Error) ||
                                   !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var bulinsInsurance = civilInsurancesResponse.CivilInsurances.BulinsCivilInsurance;
                            bulinsInsurance.InsuranceCompanyName = GlobalConstants.Bulins;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Bulins);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(bulinsInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance.Error) ||
                                   !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var bulstradInsurance = civilInsurancesResponse.CivilInsurances.BulstradCivilInsurance;
                            bulstradInsurance.InsuranceCompanyName = GlobalConstants.Bulstrad;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Bulstrad);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(bulstradInsurance);
                            }

                        }

                        if (civilInsurancesResponse.CivilInsurances.OzkCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.OzkCivilInsurance.Error) ||
                                   !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.OzkCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.OzkCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.OzkCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var ozkInsurance = civilInsurancesResponse.CivilInsurances.OzkCivilInsurance;
                            ozkInsurance.InsuranceCompanyName = GlobalConstants.Ozk;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Ozk);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(ozkInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance.Error) ||
                                   !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var groupamaInsurance = civilInsurancesResponse.CivilInsurances.GroupamaCivilInsurance;
                            groupamaInsurance.InsuranceCompanyName = GlobalConstants.Groupama;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Groupama);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(groupamaInsurance);
                            }
                        }

                        if (civilInsurancesResponse.CivilInsurances.LevinsCivilInsurance != null)
                        {
                            if (!string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.LevinsCivilInsurance.Error) ||
                                   !string.IsNullOrEmpty(civilInsurancesResponse.CivilInsurances.LevinsCivilInsurance.SystemMessage))
                            {
                                if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.CivilInsurances.LevinsCivilInsurance.Error))
                                {
                                    errorMessage = civilInsurancesResponse.CivilInsurances.LevinsCivilInsurance.Error;
                                }
                                isError = true;
                            }

                            var levinsInsurance = civilInsurancesResponse.CivilInsurances.LevinsCivilInsurance;
                            levinsInsurance.InsuranceCompanyName = GlobalConstants.Levins;

                            var hasDocs = this._documentService.CheckForFreeCivilDocs(GlobalConstants.Levins);

                            if (hasDocs)
                            {
                                result.InsuranceOffers.Add(levinsInsurance);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(civilInsurancesResponse.Error))
                {
                    if (Helpers.IsValidCivilExceptionMessage(civilInsurancesResponse.Error))
                    {
                        errorMessage = civilInsurancesResponse.Error;
                    }
                    isError = true;
                }

                if (isError)
                {
                    string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                    int randomNumber = Helpers.GetRandomNumber();
                    string id = $"ins-{dateStr}-{randomNumber}";
                    result.ErrorId = id;
                    result.Message = errorMessage;

                    Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, id); });
                }

                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                return result;
            }
        }

        public async Task<CivilInsurancePolicyPayload> CivilInsurancePolicy(CivilInsurancePolicyInput req)
        {
            var result = new CivilInsurancePolicyPayload();
            var sirmaRequest = new CivilInsurancePolicyRequestModel();

            if (!string.IsNullOrEmpty(req.OwnerVatNumber))
            {
                sirmaRequest.OwnerPersonType = 1;
                sirmaRequest.UiNumber = req.OwnerVatNumber;

                sirmaRequest.HasUsualDriver = 1;
                sirmaRequest.UsualExperience = req.UsualExperience;
                sirmaRequest.UiNumberUsual = req.UsualUiNumber;
                sirmaRequest.PostalCodeUsual = req.UsualPostalCode;
                sirmaRequest.UsualPersonType = 2;
                sirmaRequest.RegionUsual = req.UsualRegion;
                sirmaRequest.МunicipalityUsual = req.UsualMunicipality;
                sirmaRequest.NameUsual = req.UsualName;
                sirmaRequest.AddressUsual = req.UsualAddress;
                sirmaRequest.BirthdateUsual = req.UsualBirthdate;
                sirmaRequest.UsualCountryCode = req.UsualCountryCode;
                sirmaRequest.UsualCity = req.UsualCity;

                if (req.UsualCountryCode == GlobalConstants.BgCountryCode)
                {
                    sirmaRequest.NationalityUsual = 1;
                }
                else
                {
                    sirmaRequest.NationalityUsual = 0;
                }
            }
            else
            {
                sirmaRequest.OwnerPersonType = 2;
                sirmaRequest.UiNumber = req.OwnerUiNumber;
            }

            if (req.OwnerCountryCode == GlobalConstants.BgCountryCode)
            {
                sirmaRequest.Nationality = 1;
            }
            else
            {
                sirmaRequest.Nationality = 0;
            }

            sirmaRequest.OwnerExperience = req.OwnerExperience;
            sirmaRequest.VehiclePlateNumber = req.VehiclePlateNumber;
            sirmaRequest.VehicleUsage = req.VehicleUsage;
            sirmaRequest.RegistrationCertificateNumber = req.RegistrationCertificateNumber;
            sirmaRequest.PostalCode = req.OwnerPostalCode;
            sirmaRequest.VehicleType = req.VehicleType;
            sirmaRequest.Office = "1";
            sirmaRequest.City = req.OwnerCity;
            sirmaRequest.Region = req.OwnerRegion;
            sirmaRequest.Мunicipality = req.OwnerMunicipality;
            sirmaRequest.Name = req.OwnerName;
            sirmaRequest.Address = req.OwnerAddress;
            sirmaRequest.Installments = req.Installments;
            sirmaRequest.Birthdate = req.OwnerBirthdate;
            sirmaRequest.VehicleFirstReg = req.VehicleFirstReg;
            sirmaRequest.VehicleBrand = req.VehicleBrand;
            sirmaRequest.VehicleModel = req.VehicleModel;
            sirmaRequest.OwnerCountryCode = req.OwnerCountryCode;

            var insuranceCompany = this._insuranceCompanyService.GetCompanyByName<InsuranceCompanyModel>(req.InsuranceCompany);
            var stickerNumber = this._documentService.GetStickerNumberById(req.StickerId);
            var greenCardNumber = this._documentService.GetGreenCardNumberById(req.GreenCardId);

            sirmaRequest.Insurer = insuranceCompany.CompanyName.ToLower();
            sirmaRequest.PolicyStartDate = req.StartDate.Value.ToString("dd.MM.yyyy");
            sirmaRequest.GreencardNo = greenCardNumber;
            sirmaRequest.StickerNo = stickerNumber;

            byte insuranceType = (byte)InsuranceTypeEnum.Civil;
            byte insuranceStatus = (byte)InsuranceStatusEnum.Valid;
            decimal firstInstallmentPrice = 0;
            decimal secondInstallmentPrice = 0;
            decimal thirdInstallmentPrice = 0;
            decimal fourthInstallmentPrice = 0;

            decimal firstInstallmentTax = 0;
            decimal secondInstallmentTax = 0;
            decimal thirdInstallmentTax = 0;
            decimal fourthInstallmentTax = 0;

            decimal price = 0;
            string policyNo = string.Empty;
            byte[] receiptFileArr = null;
            string policyPdfUrl = string.Empty;
            string greenCardPdfUrl = string.Empty;
            string receiptPdfUrl = null;

            var vehicle = this._vehicleService.GetVehicleByPlateNumber<VehicleResultModel>(req.VehiclePlateNumber, req.UserId);
            string vehicleId = vehicle != null ? vehicle.Id : null;
            string vehiclePlateNumber = null;
            string vehicleModel = null;
            string vehicleBrand = null;

            if (string.IsNullOrEmpty(vehicleId))
            {
                vehiclePlateNumber = req.VehiclePlateNumber;
                vehicleBrand = req.VehicleBrand;
                vehicleModel = req.VehicleModel;
            }
            int installmentToPay = 0;

            var currency = this._currencyService.GetCurrencyByCode<CurrencyModel>(GlobalConstants.Bgn);
            var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.LocalOrderNumber);
            DateTime? currentEndDate = null;
            DateTime? policyEndDate = null;

            DateTime? secondInstallmentDate = null;
            DateTime? thirdInstallmentDate = null;
            DateTime? fourthInstallmentDate = null;

            var insuranceTypeEnum = InsuranceTypeEnum.Civil;
            string insuranceTypeDescription = Helpers.GetEnumDescription(insuranceTypeEnum);

            string requestJson = JsonConvert.SerializeObject(req);
            string responseJson = string.Empty;
            bool isError = false;

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var civilInsurancesPolicyResponse = await this._civilInsuranceService.CivilInsurancePolicy(sirmaRequest);

                if(civilInsurancesPolicyResponse != null)
                {
                    responseJson = JsonConvert.SerializeObject(civilInsurancesPolicyResponse);

                    if (civilInsurancesPolicyResponse.Success == (int)GeneralStatusEnum.Success)
                    {
                        if (civilInsurancesPolicyResponse.Info != null)
                        {
                            if (civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FirstInstallment != null &&
                                !string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FirstInstallment.PaymentDate))
                            {
                                firstInstallmentPrice = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FirstInstallment.PremiumWithTax;
                                firstInstallmentTax = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FirstInstallment.Tax;
                            }

                            if (civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment != null &&
                                !string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.PaymentDate))
                            {
                                secondInstallmentPrice = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.PremiumWithTax;
                                secondInstallmentTax = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.Tax;
                                secondInstallmentDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.PaymentDate);
                                currentEndDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.PaymentDate);

                                installmentToPay = 2;
                            }

                            if (civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment != null &&
                                !string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment.PaymentDate))
                            {
                                thirdInstallmentPrice = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment.PremiumWithTax;
                                thirdInstallmentDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment.PaymentDate);

                                thirdInstallmentTax = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment.Tax;
                            }

                            if (civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment != null &&
                                !string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment.PaymentDate))
                            {
                                fourthInstallmentPrice = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment.PremiumWithTax;
                                fourthInstallmentDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment.PaymentDate);

                                fourthInstallmentTax = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment.Tax;
                            }

                            if (!string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.Issue.Policy.EndDate))
                            {
                                policyEndDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.Issue.Policy.EndDate);
                            }
                            else
                            {
                                policyEndDate = req.StartDate.Value.AddYears(1).AddDays(-1);
                            }

                            if (fourthInstallmentPrice == 0 && thirdInstallmentPrice == 0 && secondInstallmentPrice == 0)
                            {
                                currentEndDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.Issue.Policy.EndDate);

                            }

                            result.Success = civilInsurancesPolicyResponse.Success;
                            price = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.PremiumWithTax;

                            result.PolicyNo = civilInsurancesPolicyResponse.Info.Issue.Policy.PolicyNumber;

                            policyNo = civilInsurancesPolicyResponse.Info.Issue.Policy.PolicyNumber;
                            var policyNoFolder = policyNo.Replace('/', '_');

                            policyPdfUrl = $"{req.MainUserId}\\{policyNoFolder}\\policy_{policyNoFolder}.pdf";
                            greenCardPdfUrl = $"{req.MainUserId}\\{policyNoFolder}\\greenCard.pdf";

                            var policyPdfHash = civilInsurancesPolicyResponse.Info.Issue.Policy.PdfUrl;
                            var greenCardPdfHash = civilInsurancesPolicyResponse.Info.Issue.GreenCard.PdfUrl;

                            var policyFileArr = await this.GetCivilInsuranceDocFile(policyPdfHash);
                            var greenCardFileArr = await this.GetCivilInsuranceDocFile(greenCardPdfHash);

                            var notePdfHash = civilInsurancesPolicyResponse.Info.Note.Url;
                            if (!string.IsNullOrEmpty(notePdfHash))
                            {
                                receiptPdfUrl = $"{req.MainUserId}\\{policyNoFolder}\\receipt.pdf";
                                receiptFileArr = await this.GetCivilInsuranceDocFile(notePdfHash);
                            }

                            int installment = req.Installments;
                            string insuranceId = await this.CreateInsurance(req.StartDate.Value, policyEndDate.Value, insuranceStatus, insuranceCompany.Id.ToString(), price,
                            insuranceType, currency.Id.ToString(), currentEndDate.Value, policyNo, installment, policyPdfUrl, installmentToPay, receiptPdfUrl);

                            if (string.IsNullOrEmpty(insuranceId))
                            {
                                throw new Exception("Insurance creation failed.");
                            }

                            string insurancePaymentInfoId = await this.CreateInsurancePaymentInformation(insuranceId, paymentInfo.Id);

                            if (string.IsNullOrEmpty(insurancePaymentInfoId))
                            {
                                throw new Exception("InsurancePaymentInformation creation failed.");
                            }

                            string userInsurancesResult = string.Empty;

                            if (req.UserId != req.MainUserId)
                            {
                                userInsurancesResult = await this.CreateUsersInsurances(req.MainUserId, insuranceId, false, true, false, false);

                                if (!string.IsNullOrEmpty(req.UserId) && req.UserId != GlobalConstants.New)
                                {
                                    userInsurancesResult = await this.CreateUsersInsurances(req.UserId, insuranceId, true, false, false, false);
                                }
                                else
                                {
                                    string userId = await this.AddInsuredUser(req.Email, req.OwnerUiNumber, req.OwnerVatNumber, req.Phone, req.OwnerName, null, null);

                                    if (!string.IsNullOrEmpty(userId))
                                    {
                                        userInsurancesResult = await this.CreateUsersInsurances(userId, insuranceId, true, false, false, false);
                                    }
                                }
                            }
                            else
                            {
                                userInsurancesResult = await this.CreateUsersInsurances(req.MainUserId, insuranceId, true, true, false, false);
                            }

                            if (string.IsNullOrEmpty(userInsurancesResult))
                            {
                                throw new Exception("UserInsurancces creation failed.");
                            }

                            if (!string.IsNullOrEmpty(req.UsualName))
                            {
                                string userInsurancesUsual = string.Empty;

                                if (req.MainUserId != req.UsualUserId)
                                {
                                    if (!string.IsNullOrEmpty(req.UsualUserId) && req.UsualUserId != GlobalConstants.New)
                                    {
                                        userInsurancesUsual = await this.CreateUsersInsurances(req.UsualUserId, insuranceId, false, false, true, false);
                                    }
                                    else
                                    {
                                        string userId = await this.AddInsuredUser(req.Email, req.UsualUiNumber, null, req.Phone, req.UsualName, null, null);

                                        if (!string.IsNullOrEmpty(userId))
                                        {
                                            userInsurancesUsual = await this.CreateUsersInsurances(userId, insuranceId, false, false, true, false);
                                        }
                                    }
                                }
                                else
                                {
                                    userInsurancesUsual = await this.CreateUsersInsurances(req.MainUserId, insuranceId, false, false, true, false);
                                }

                                if (userInsurancesUsual == null)
                                {
                                    throw new Exception("UserInsurancces creation failed.");
                                }
                            }

                            string civilInsurance = await this.CreateCivilInsurance(firstInstallmentPrice, secondInstallmentPrice,
                                thirdInstallmentPrice, fourthInstallmentPrice, req.GreenCardId, req.StickerId, insuranceId, secondInstallmentDate,
                                thirdInstallmentDate, fourthInstallmentDate, greenCardPdfUrl, req.UsualUserId, req.UserId, vehicleId,
                                firstInstallmentTax, secondInstallmentTax, thirdInstallmentTax, fourthInstallmentTax,
                                vehicleModel, vehicleBrand, vehiclePlateNumber);

                            if (string.IsNullOrEmpty(civilInsurance))
                            {
                                throw new Exception("Civil Insurance creation failed.");
                            }

                            var savePolicy = this.SaveFile(policyPdfUrl, policyFileArr);
                            var saveGreenCard = this.SaveFile(greenCardPdfUrl, greenCardFileArr);

                            if(receiptFileArr != null)
                            {
                                var saveReceipt = this.SaveFile(receiptPdfUrl, receiptFileArr);
                            }

                            Task.Run(async () => { await SendInsuredUserEmail(req.Email, (int)insuranceType, req.OwnerName, policyNo, policyFileArr, greenCardFileArr, receiptFileArr, req.InsuranceCompany); });
                            Task.Run(async () => { await SendBrokerEmail((int)insuranceType, policyNo, insuranceTypeDescription, policyFileArr, greenCardFileArr, receiptFileArr); });

                        }
                    }
                    else
                    {
                        string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                        int randomNumber = Helpers.GetRandomNumber();
                        string id = $"ins-{dateStr}-{randomNumber}";
                        result.ErrorId = id;

                        if (Helpers.IsValidCivilExceptionMessage(civilInsurancesPolicyResponse.Error))
                        {
                            result.Message = civilInsurancesPolicyResponse.Error;
                        };

                        Task.Run(async () => { await this._documentService.StickerGreenCardErrorOccured(req.StickerId, req.GreenCardId); });
                        Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, id); });
                    }
                }
                else
                {
                    throw new Exception("Civil insurance policy unsuccess");
                }

                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                this.LogError(ex.Message, $"{ex.StackTrace}");
                Task.Run(async () => { await this._documentService.StickerGreenCardErrorOccured(req.StickerId, req.GreenCardId); });
                transaction.Rollback();
                return new CivilInsurancePolicyPayload();
            }
        }

        public async Task<CivilInsuranceLongPolicyPayload> CivilInsuranceLongPolicy(CivilInsuranceLongPolicyInput req)
        {
            var result = new CivilInsuranceLongPolicyPayload();
            var sirmaRequest = new CivilInsuranceLongPolicyRequestModel();

            if (!string.IsNullOrEmpty(req.OwnerVatNumber))
            {
                sirmaRequest.OwnerPersonType = 1;
                sirmaRequest.UiNumber = req.OwnerVatNumber;

                sirmaRequest.HasUsualDriver = 1;
                sirmaRequest.UsualExperience = req.UsualExperience;
                sirmaRequest.UiNumberUsual = req.UsualUiNumber;
                sirmaRequest.PostalCodeUsual = req.UsualPostalCode;
                sirmaRequest.UsualPersonType = 2;
                sirmaRequest.RegionUsual = req.UsualRegion;
                sirmaRequest.МunicipalityUsual = req.UsualMunicipality;
                sirmaRequest.NameUsual = req.UsualName;
                sirmaRequest.AddressUsual = req.UsualAddress;
                sirmaRequest.BirthdateUsual = req.UsualBirthdate;
                sirmaRequest.UsualCountryCode = req.UsualCountryCode;
                sirmaRequest.UsualGuilt = req.UsualGuilt;
                sirmaRequest.UsualCity = req.UsualCity;

                if (req.UsualCountryCode == GlobalConstants.BgCountryCode)
                {
                    sirmaRequest.NationalityUsual = 1;
                }
                else
                {
                    sirmaRequest.NationalityUsual = 0;
                }
            }
            else
            {
                sirmaRequest.OwnerPersonType = 2;
                sirmaRequest.UiNumber = req.OwnerUiNumber;
            }

            if (req.OwnerCountryCode == GlobalConstants.BgCountryCode)
            {
                sirmaRequest.Nationality = 1;
            }
            else
            {
                sirmaRequest.Nationality = 0;
            }

            sirmaRequest.OwnerExperience = req.OwnerExperience;
            sirmaRequest.VehiclePlateNumber = req.VehiclePlateNumber;
            sirmaRequest.VehicleUsage = req.VehicleUsage;
            sirmaRequest.RegistrationCertificateNumber = req.RegistrationCertificateNumber;
            sirmaRequest.PostalCode = req.OwnerPostalCode;
            sirmaRequest.VehicleType = req.VehicleType;
            sirmaRequest.Office = "0";
            sirmaRequest.City = req.OwnerCity;
            sirmaRequest.Region = req.OwnerRegion;
            sirmaRequest.Мunicipality = req.OwnerMunicipality;
            sirmaRequest.Name = req.OwnerName;
            sirmaRequest.Address = req.OwnerAddress;
            sirmaRequest.Installments = req.Installments;
            sirmaRequest.Birthdate = req.OwnerBirthdate;
            sirmaRequest.VehicleFirstReg = req.VehicleFirstReg;
            sirmaRequest.VehicleBrand = req.VehicleBrand;
            sirmaRequest.VehicleModel = req.VehicleModel;
            sirmaRequest.OwnerCountryCode = req.OwnerCountryCode;

            sirmaRequest.OwnerGuilt = req.OwnerGuilt;
            sirmaRequest.VehicleVin = req.Vin;
            sirmaRequest.EngineVolume = req.VehicleEngineVolume;
            sirmaRequest.VehicleBodyType = req.VehicleBodyType;
            sirmaRequest.VehiclePlaces = req.VehiclePlaces;
            sirmaRequest.VehicleColor = req.VehicleColor;
            sirmaRequest.VehicleEngineType = req.VehicleEngineType;
            sirmaRequest.VehicleSteeringWheel = req.VehicleSteeringWheel;
            sirmaRequest.VehicleKilowatts = req.VehicleKilowatts;

            var insuranceCompany = this._insuranceCompanyService.GetCompanyByName<InsuranceCompanyModel>(req.InsuranceCompany);
            var stickerNumber = this._documentService.GetStickerNumberById(req.StickerId);
            var greenCardNumber = this._documentService.GetGreenCardNumberById(req.GreenCardId);

            sirmaRequest.Insurer = insuranceCompany.CompanyName.ToLower();
            sirmaRequest.PolicyStartDate = req.StartDate.Value.ToString("dd.MM.yyyy");
            sirmaRequest.GreencardNo = greenCardNumber;
            sirmaRequest.StickerNo = stickerNumber;

            byte insuranceType = (byte)InsuranceTypeEnum.Civil;
            byte insuranceStatus = (byte)InsuranceStatusEnum.Valid;
            decimal firstInstallmentPrice = 0;
            decimal secondInstallmentPrice = 0;
            decimal thirdInstallmentPrice = 0;
            decimal fourthInstallmentPrice = 0;

            decimal firstInstallmentTax = 0;
            decimal secondInstallmentTax = 0;
            decimal thirdInstallmentTax = 0;
            decimal fourthInstallmentTax = 0;

            decimal price = 0;
            string policyNo = string.Empty;
            string policyPdfUrl = string.Empty;
            string greenCardPdfUrl = string.Empty;
            string receiptPdfUrl = null;
            byte[] receiptFileArr = null;
            string noteNumber = string.Empty;

            var vehicle = this._vehicleService.GetVehicleByPlateNumber<VehicleResultModel>(req.VehiclePlateNumber, req.UserId);
            string vehicleId = vehicle != null ? vehicle.Id : null;
            string vehiclePlateNumber = null;
            string vehicleModel = null;
            string vehicleBrand = null;

            if (string.IsNullOrEmpty(vehicleId))
            {
                vehiclePlateNumber = req.VehiclePlateNumber;
                vehicleBrand = req.VehicleBrand;
                vehicleModel = req.VehicleModel;
            }
            int installmentToPay = 0;

            var currency = this._currencyService.GetCurrencyByCode<CurrencyModel>(GlobalConstants.Bgn);
            var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.LocalOrderNumber);
            DateTime? currentEndDate = null;
            DateTime? policyEndDate = null;

            DateTime? secondInstallmentDate = null;
            DateTime? thirdInstallmentDate = null;
            DateTime? fourthInstallmentDate = null;

            var insuranceTypeEnum = InsuranceTypeEnum.Civil;
            string insuranceTypeDescription = Helpers.GetEnumDescription(insuranceTypeEnum);

            string requestJson = JsonConvert.SerializeObject(req);
            string responseJson = string.Empty;
            bool isError = false;

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var civilInsurancesPolicyResponse = await this._civilInsuranceService.CivilInsuranceLongPolicy(sirmaRequest);

                if (civilInsurancesPolicyResponse != null)
                {
                    responseJson = JsonConvert.SerializeObject(civilInsurancesPolicyResponse);

                    if (civilInsurancesPolicyResponse != null && civilInsurancesPolicyResponse.Success == (int)GeneralStatusEnum.Success)
                    {
                        if (civilInsurancesPolicyResponse.Info != null)
                        {
                            if (civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FirstInstallment != null &&
                                    !string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FirstInstallment.PaymentDate))
                            {
                                firstInstallmentPrice = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FirstInstallment.PremiumWithTax;
                                firstInstallmentTax = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FirstInstallment.Tax;
                            }

                            if (civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment != null &&
                                    !string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.PaymentDate))
                            {
                                secondInstallmentPrice = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.PremiumWithTax;
                                secondInstallmentTax = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.Tax;
                                secondInstallmentDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.PaymentDate);
                                currentEndDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.SecondInstallment.PaymentDate);

                                installmentToPay = 2;
                            }

                            if (civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment != null &&
                                    !string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment.PaymentDate))
                            {
                                thirdInstallmentPrice = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment.PremiumWithTax;
                                thirdInstallmentDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment.PaymentDate);

                                thirdInstallmentTax = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.ThirdInstallment.Tax;
                            }

                            if (civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment != null &&
                                     !string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment.PaymentDate))
                            {
                                fourthInstallmentPrice = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment.PremiumWithTax;
                                fourthInstallmentDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment.PaymentDate);

                                fourthInstallmentTax = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Installments.FourthInstallment.Tax;
                            }

                            if (!string.IsNullOrEmpty(civilInsurancesPolicyResponse.Info.Issue.Policy.EndDate))
                            {
                                policyEndDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.Issue.Policy.EndDate);
                            }
                            else
                            {
                                policyEndDate = req.StartDate.Value.AddYears(1).AddDays(-1);
                            }

                            if (fourthInstallmentPrice == 0 && thirdInstallmentPrice == 0 && secondInstallmentPrice == 0)
                            {
                                currentEndDate = DateTime.Parse(civilInsurancesPolicyResponse.Info.Issue.Policy.EndDate);
                            }

                            result.Success = civilInsurancesPolicyResponse.Success;
                            price = civilInsurancesPolicyResponse.Info.CivilInsuranceCalculation.Premium;

                            result.PolicyNo = civilInsurancesPolicyResponse.Info.Issue.Policy.PolicyNumber;
                            policyNo = civilInsurancesPolicyResponse.Info.Issue.Policy.PolicyNumber;

                            var policyNoFolder = policyNo.Replace('/', '_');

                            policyPdfUrl = $"{req.MainUserId}\\{policyNoFolder}\\policy.pdf";
                            greenCardPdfUrl = $"{req.MainUserId}\\{policyNoFolder}\\greenCard.pdf";

                            var policyPdfHash = civilInsurancesPolicyResponse.Info.Issue.Policy.PdfUrl;
                            var greenCardPdfHash = civilInsurancesPolicyResponse.Info.Issue.GreenCard.PdfUrl;

                            var policyFileArr = await this.GetCivilInsuranceDocFile(policyPdfHash);
                            var greenCardFileArr = await this.GetCivilInsuranceDocFile(greenCardPdfHash);

                            var notePdfHash = civilInsurancesPolicyResponse.Info.Note.Url;
                            if (!string.IsNullOrEmpty(notePdfHash))
                            {
                                receiptPdfUrl = $"{req.MainUserId}\\{policyNoFolder}\\receipt.pdf";
                                receiptFileArr = await this.GetCivilInsuranceDocFile(notePdfHash);
                            }

                            int installment = req.Installments;
                            string insuranceId = await this.CreateInsurance(req.StartDate.Value, policyEndDate.Value, insuranceStatus, insuranceCompany.Id.ToString(), price,
                            insuranceType, currency.Id.ToString(), currentEndDate.Value, policyNo, installment, policyPdfUrl, installmentToPay, receiptPdfUrl);

                            if (string.IsNullOrEmpty(insuranceId))
                            {
                                throw new Exception("Insurance creation failed.");
                            }

                            string insurancePaymentInfoId = await this.CreateInsurancePaymentInformation(insuranceId, paymentInfo.Id);

                            if (string.IsNullOrEmpty(insurancePaymentInfoId))
                            {
                                throw new Exception("InsurancePaymentInformation creation failed.");
                            }

                            string userInsurancesResult = string.Empty;

                            if (req.UserId != req.MainUserId)
                            {
                                userInsurancesResult = await this.CreateUsersInsurances(req.MainUserId, insuranceId, false, true, false, false);

                                if (!string.IsNullOrEmpty(req.UserId) && req.UserId != GlobalConstants.New)
                                {
                                    userInsurancesResult = await this.CreateUsersInsurances(req.UserId, insuranceId, true, false, false, false);
                                }
                                else
                                {
                                    string userId = await this.AddInsuredUser(req.Email, req.OwnerUiNumber, req.OwnerVatNumber, req.Phone, req.OwnerName, null, null);

                                    if (!string.IsNullOrEmpty(userId))
                                    {
                                        userInsurancesResult = await this.CreateUsersInsurances(userId, insuranceId, true, false, false, false);
                                    }
                                }
                            }
                            else
                            {
                                userInsurancesResult = await this.CreateUsersInsurances(req.MainUserId, insuranceId, true, true, false, false);
                            }

                            if (string.IsNullOrEmpty(userInsurancesResult))
                            {
                                throw new Exception("UserInsurancces creation failed.");
                            }

                            if (!string.IsNullOrEmpty(req.UsualName))
                            {
                                string userInsurancesUsual = string.Empty;

                                if (req.MainUserId != req.UsualUserId)
                                {
                                    if (!string.IsNullOrEmpty(req.UsualUserId) && req.UsualUserId != GlobalConstants.New)
                                    {
                                        userInsurancesUsual = await this.CreateUsersInsurances(req.UsualUserId, insuranceId, false, false, true, false);
                                    }
                                    else
                                    {
                                        string userId = await this.AddInsuredUser(req.Email, req.UsualUiNumber, null, req.Phone, req.UsualName, null, null);

                                        if (!string.IsNullOrEmpty(userId))
                                        {
                                            userInsurancesUsual = await this.CreateUsersInsurances(req.UsualUserId, insuranceId, false, false, true, false);
                                        }
                                    }
                                }
                                else
                                {
                                    userInsurancesUsual = await this.CreateUsersInsurances(req.MainUserId, insuranceId, false, false, true, false);
                                }

                                if (userInsurancesUsual == null)
                                {
                                    throw new Exception("UserInsurancces creation failed.");
                                }
                            }

                            string civilInsurance = await this.CreateCivilInsurance(firstInstallmentPrice, secondInstallmentPrice,
                                thirdInstallmentPrice, fourthInstallmentPrice, req.GreenCardId, req.StickerId, insuranceId, secondInstallmentDate,
                                thirdInstallmentDate, fourthInstallmentDate, greenCardPdfUrl, req.UsualUserId, req.UserId, vehicleId,
                                firstInstallmentTax, secondInstallmentTax, thirdInstallmentTax, fourthInstallmentTax,
                                vehicleModel, vehicleBrand, vehiclePlateNumber);

                            if (string.IsNullOrEmpty(civilInsurance))
                            {
                                throw new Exception("Civil Insurance creation failed.");
                            }

                            var savePolicy = this.SaveFile(policyPdfUrl, policyFileArr);
                            var saveGreenCard = this.SaveFile(greenCardPdfUrl, greenCardFileArr);

                            if (receiptFileArr != null)
                            {
                                var saveReceipt = this.SaveFile(receiptPdfUrl, receiptFileArr);
                            }

                            Task.Run(async () => { await SendInsuredUserEmail(req.Email, (int)insuranceType, req.OwnerName, policyNo, policyFileArr, greenCardFileArr, receiptFileArr, req.InsuranceCompany); });
                            Task.Run(async () => { await SendBrokerEmail((int)insuranceType, policyNo, insuranceTypeDescription, policyFileArr, greenCardFileArr, receiptFileArr); });
                        }
                    }
                    else
                    {
                        throw new Exception("Civil insurance policy unsuccess");
                    }
                }
                else
                {
                    string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                    int randomNumber = Helpers.GetRandomNumber();
                    string id = $"ins-{dateStr}-{randomNumber}";
                    result.ErrorId = id;

                    if (Helpers.IsValidCivilExceptionMessage(civilInsurancesPolicyResponse.Error))
                    {
                        result.Message = civilInsurancesPolicyResponse.Error;
                    }

                    Task.Run(async () => { await this._documentService.StickerGreenCardErrorOccured(req.StickerId, req.GreenCardId); });
                    Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, id); });
                }

                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                this.LogError(ex.Message, $"{ex.StackTrace}");
                Task.Run(async () => { await this._documentService.StickerGreenCardErrorOccured(req.StickerId, req.GreenCardId); });
                transaction.Rollback();
                return new CivilInsuranceLongPolicyPayload();
            }
        }

        public async Task<CivilInsuranceInstallmentPayload> CivilInsuranceInstallment(CivilInsuranceInstallmentInput req)
        {
            var result = new CivilInsuranceInstallmentPayload();

            var insurance = this.GetInsuranceById(req.InsuranceId);

            var mainUserInfo = this.GetInsurerUserInfo(req.UserId, req.InsuranceId);
            var names = mainUserInfo.FirstName + " " + mainUserInfo.LastName;

            var insuranceTypeEnum = InsuranceTypeEnum.Civil;
            string insuranceTypeDescription = Helpers.GetEnumDescription(insuranceTypeEnum);

            if (insurance != null && insurance.Type == (byte)InsuranceTypeEnum.Civil)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.LocalOrderNumber);
                var stickerNumber = this._documentService.GetStickerNumberById(req.StickerId);
                var greenCardNumber = this._documentService.GetGreenCardNumberById(req.GreenCardId);

                var sirmaRequest = await this.CreateSirmaInsuranceInstallmentRequest(req, stickerNumber, greenCardNumber, insurance);
                int installmentToPay = insurance.InstallmentToPay;
                string greenCardStoreNumber = installmentToPay.ToString();
                DateTime? currentEndDate = null;
                string greenCardPdf = string.Empty;


                if (insurance.InstallmentToPay == insurance.InstallmentsNumber)
                {
                    installmentToPay = 0;
                    currentEndDate = insurance.PolicyEndDate;
                }
                else
                {
                    if (insurance.InstallmentToPay == 2)
                    {
                        installmentToPay++;
                        currentEndDate = insurance.CivilInsurance.ThirdInstallmentDate;
                    }
                    else if (insurance.InstallmentToPay == 3)
                    {
                        installmentToPay++;
                        currentEndDate = insurance.CivilInsurance.FourthInstallmentDate;
                    }
                }

                string requestJson = JsonConvert.SerializeObject(req);
                string responseJson = string.Empty;
                bool isError = false;

                try
                {
                    var civilInsurancesPolicyResponse = await this._civilInsuranceService.CivilInsuranceInstallmentPay(sirmaRequest);

                    if (civilInsurancesPolicyResponse.Success == (int)GeneralStatusEnum.Success)
                    {
                        string greenCardFile = $"{greenCardNumber}_{greenCardStoreNumber}";
                       
                        var policyNoFolder = insurance.PolicyNo.Replace('/', '_');
                        greenCardPdf = $"{req.UserId}\\{policyNoFolder}\\{greenCardFile}.pdf";

                        var saveGreenCard = this.SaveFile(greenCardPdf);

                        var updateInsurance = await this.UpdateInsurance(insurance.Id, currentEndDate.Value, installmentToPay);

                        if (string.IsNullOrEmpty(updateInsurance))
                        {
                            throw new Exception("Update insurance failed");
                        }

                        var updateCivilInsurance = await this.UpdateCivilInsurance(insurance.CivilInsurance.Id.ToString(), greenCardPdf,
                            req.StickerId, req.GreenCardId);

                        if (string.IsNullOrEmpty(updateCivilInsurance))
                        {
                            throw new Exception("Update Civil insurance failed");
                        }

                        var createInsurancePaymentInfo = await this.CreateInsurancePaymentInformation(insurance.Id.ToString(), paymentInfo.Id.ToString());

                        if (string.IsNullOrEmpty(createInsurancePaymentInfo))
                        {
                            throw new Exception("Create InsurancePaymentInformations  failed");
                        }

                        result.Code = (int)GeneralStatusEnum.Success;
                    }
                    else
                    {
                        string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                        int randomNumber = Helpers.GetRandomNumber();
                        string id = $"ins-{dateStr}-{randomNumber}";
                        result.ErrorId = id;

                        if (Helpers.IsValidCivilExceptionMessage(civilInsurancesPolicyResponse.Error))
                        {
                            result.Message = civilInsurancesPolicyResponse.Error;
                        }

                        Task.Run(async () => { await this._documentService.StickerGreenCardErrorOccured(req.StickerId, req.GreenCardId); });
                        Task.Run(async () => { this.LogInsuranceError(requestJson, responseJson, id); });
                    }


                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    this.LogError(ex.Message, $"{ex.StackTrace}");
                    Task.Run(async () => { await this._documentService.StickerGreenCardErrorOccured(req.StickerId, req.GreenCardId); });
                    transaction.Rollback();
                }
            }

            return result;
        }

        public int GetCivilInsurancesCount()
        {
            var insCount = this._insuranceRepository
                .AllAsNoTracking()
                .Where(i => i.Type == (int)InsuranceTypeEnum.Civil).Count();

            return insCount;
        }
        public int GetCivilInsurancesCountByCompanyName(string companyName)
        {
            var insCount = this._insuranceRepository
                     .AllAsNoTracking()
                     .Where(i => i.Type == (int)InsuranceTypeEnum.Civil && i.InsuranceCompany.CompanyName.Equals(companyName)).Count();

            return insCount;
        }

        public async Task<string> UpdateCivilInsurance(string civilInsuranceId, string greenCardPdf, string stickerId, string greenCardId)
        {
            string result = string.Empty;

            var currentCivilInsuranceId = Helpers.ParseStringToGuid(civilInsuranceId);
            var currentStickerId = Helpers.ParseStringToGuid(stickerId);
            var currentGreenCardId = Helpers.ParseStringToGuid(greenCardId);

            var civilInsurance = this._civilInsurancesRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == currentCivilInsuranceId);

            if (civilInsurance != null)
            {
                civilInsurance.GreenCardPdfUrl = greenCardPdf;
                civilInsurance.StickerId = currentStickerId;
                civilInsurance.GreenCardId = currentGreenCardId;

                this._civilInsurancesRepository.Update(civilInsurance);
                await this._civilInsurancesRepository.SaveChangesAsync();

                result = civilInsurance.Id.ToString();
            }

            return result;
        }

        public async Task<string> CreateCivilInsurance(decimal firstInstallmentPrice, decimal secondInstallmentPrice,
            decimal thirdInstallmentPrice, decimal fourthInstallmentPrice, string greenCardId,
            string stickerId, string insuranceId, DateTime? secondInstallmentDate, DateTime? thirdInstallmentDate,
            DateTime? fourthInstallmentDate, string greenCardPdfUrl, string usualDriverId, string userId,
            string vehicleId, decimal firstInstallmentTax, decimal secondInstallmentTax, decimal thirdInstallmentTax,
            decimal fourthInstallmentTax, string vehicleModel, string vehicleBrand, string vehiclePlateNumber)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);
            var currentStickerId = Helpers.ParseStringToGuid(stickerId);
            var currentGreenCardId = Helpers.ParseStringToGuid(greenCardId);

            var civilInsurance = new CivilInsurance
            {
                Id = Guid.NewGuid(),
                FirstInstallmentPrice = firstInstallmentPrice,
                SecondInstallmentPrice = secondInstallmentPrice,
                ThirdInstallmentPrice = thirdInstallmentPrice,
                FourthInstallmentDate = fourthInstallmentDate.HasValue ? fourthInstallmentDate.Value : null,
                GreenCardId = currentGreenCardId,
                StickerId = currentStickerId,
                InsuranceId = currentInsuranceId,
                SecondInstallmentDate = secondInstallmentDate.HasValue ? secondInstallmentDate.Value : null,
                ThirdInstallmentDate = thirdInstallmentDate.HasValue ? thirdInstallmentDate.Value : null,
                FourthInstallmentPrice = fourthInstallmentPrice,
                GreenCardPdfUrl = greenCardPdfUrl,
                VehicleId = string.IsNullOrEmpty(vehicleId) ? null : Helpers.ParseStringToGuid(vehicleId),
                FirstInstallmentTax = firstInstallmentTax,
                SecondInstallmentTax = secondInstallmentTax,
                ThirdInstallmentTax = thirdInstallmentTax,
                FourthInstallmentTax = fourthInstallmentTax,
                VehicleBrand = vehicleBrand,
                VehicleModel = vehicleModel,
                VehiclePlateNumber = vehiclePlateNumber
            };

            await this._civilInsurancesRepository.AddAsync(civilInsurance);

            await this._civilInsurancesRepository.SaveChangesAsync();

            return civilInsurance.Id.ToString();
        }

        private async Task<CivilInsuranceInstallmentRequestModel> CreateSirmaInsuranceInstallmentRequest(CivilInsuranceInstallmentInput input,
            string stickerNo, string greenCardNo, InsuranceModel insurance)
        {
            var sirmaRequest = new CivilInsuranceInstallmentRequestModel();

            string maturity = string.Empty;
            decimal tax = 0;
            decimal amount = 0;

            var insurerUserId = insurance.InsuredUsers.Where(x => x.IsInsurer == true).Select(x => x.UserId).FirstOrDefault();
            var insurer = this._userService.GetUserById(insurerUserId).FirstOrDefault();

            string insurerVat = insurer.VatNumber;
            string insurerUin = insurer.UiNumber;

            if (insurance != null && insurance.Type == (byte)InsuranceTypeEnum.Civil)
            {
                if (insurance.InstallmentToPay == 2)
                {
                    maturity = insurance.CivilInsurance.SecondInstallmentDate.Value.ToString("dd.MM.yyyy");
                    tax = insurance.CivilInsurance.SecondInstallmentTax;
                    amount = insurance.CivilInsurance.SecondInstallmentPrice;
                }
                else if (insurance.InstallmentToPay == 3)
                {
                    maturity = insurance.CivilInsurance.ThirdInstallmentDate.Value.ToString("dd.MM.yyyy");
                    tax = insurance.CivilInsurance.ThirdInstallmentTax;
                    amount = insurance.CivilInsurance.ThirdInstallmentPrice;
                }
                else if (insurance.InstallmentToPay == 4)
                {
                    maturity = insurance.CivilInsurance.FourthInstallmentDate.Value.ToString("dd.MM.yyyy");
                    tax = insurance.CivilInsurance.FourthInstallmentTax;
                    amount = insurance.CivilInsurance.FourthInstallmentTax;
                }

                sirmaRequest.Insurer = insurance.InsuranceCompany.CompanyName.ToLower();
                sirmaRequest.UiNumber = string.IsNullOrEmpty(insurerUin) ? insurerVat : insurerUin;
                sirmaRequest.OwnerPersonType = string.IsNullOrEmpty(insurerUin) ? 1 : 2;
                sirmaRequest.PolicyNo = insurance.PolicyNo;
                sirmaRequest.InstallmentToPay = insurance.InstallmentToPay;
                sirmaRequest.StickerNo = stickerNo;
                sirmaRequest.Maturity = maturity;
                sirmaRequest.Tax = tax;
                sirmaRequest.Amount = amount;
                sirmaRequest.GreencardNo = greenCardNo;
                sirmaRequest.Installments = insurance.InstallmentsNumber;
            }

            return sirmaRequest;
        }

        public CheckVehicleCivilInsuranceAllowedPayload CheckVehicleNewCivilInsuranceAllowed(string vehiclePlateNumber)
        {
            var result = new CheckVehicleCivilInsuranceAllowedPayload();
            var currentAllowedDate = DateTime.Now.AddDays(-30);

            var ins = this._civilInsurancesRepository
                .AllAsNoTracking()
                .Include(x => x.Vehicle)
                .Include(x => x.Insurance)
                .Where(x => x.Vehicle.PlateNumber == vehiclePlateNumber &&
                            x.Insurance.PolicyEndDate >= currentAllowedDate &&
                            x.Insurance.PolicyEndDate <= DateTime.Now)
                .Select(x => new InsuranceModel
                {
                    PolicyEndDate = x.Insurance.PolicyEndDate,
                })
                .FirstOrDefault();

            result.IsForbidden = ins == null ? false : true;
            result.EndDate = ins == null ? null : ins.PolicyEndDate;

            return result;
        }

        public byte[] GetCivilInsuranceGreenCard(string userId, string insuranceId)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var policyUrl = this._civilInsurancesRepository
                .AllAsNoTracking()
                .Include(x => x.Insurance)
                .Include(x => x.Insurance.UsersInsurances)
                .Where(x => x.Insurance.Id == currentInsuranceId && x.Insurance.UsersInsurances.Any(y => y.UserId == currentUserId))
                .Select(x => x.GreenCardPdfUrl)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(policyUrl))
            {
                return this._readWriteService.GetFile(policyUrl);
            }

            return null;
        }

        public async Task<byte[]> GetCivilInsuranceDocFile(string hash)
        {
            var file = await this._civilInsuranceService.CivilInsuranceGetPdf(hash);
            return file;
        }

        #endregion

        #region Travel

        public async Task<TravelCalculationResultModel> UniqaTravelCalculation(TravelCalculationInput req)
        {
            var result = new TravelCalculationResultModel();

            try
            {
                var uniqaResponse = await this._uniqaService.TravelCalculation(req);

                if (uniqaResponse != null)
                {
                    result = uniqaResponse;
                    result.CompanyName = GlobalConstants.Uniqa;
                }

                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                return result;

            }
        }

        public async Task<TravelPolicyPayload> UniqaTravelPolicy(TravelPolicyInput req)
        {
            var result = new TravelPolicyPayload();
            var insuranceTypeEnum = InsuranceTypeEnum.Travel;
            string insuranceTypeDescription = Helpers.GetEnumDescription(insuranceTypeEnum);

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var insuranceCompany = this._insuranceCompanyService.GetCompanyByName<InsuranceCompanyModel>(req.InsuranceCompany);
                var currency = this._currencyService.GetCurrencyByCode<CurrencyModel>(GlobalConstants.Eur);
                var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.LocalOrderNumber);
                byte insuranceType = (byte)InsuranceTypeEnum.Travel;
                byte insuranceStatus = (byte)InsuranceStatusEnum.Valid;

                var orderIdResult = await this._uniqaService.TravelOffer(req);

                if (orderIdResult != null && !string.IsNullOrEmpty(orderIdResult.OfferId))
                {
                    var policyResult = await this._uniqaService.IssueTravelPolicyRequest(orderIdResult.OfferId);

                    if (policyResult != null && policyResult.Success)
                    {
                        var travelInfo = await this._uniqaService.TravelOrderInfo(orderIdResult.OfferId);

                        if (travelInfo != null && !string.IsNullOrEmpty(travelInfo.PolicuNo))
                        {
                            string uniqaInsuranceType = GlobalConstants.TravelInsuranceType;
                            bool uniqaInsuranceGroup = GlobalConstants.TravelInsuranceGroup;

                            var receipt = new PayUniqaInstallmentResultModel();
                            var policyInfo = await this._uniqaService.GetPolicyInstallmentInfo(travelInfo.PolicuNo, uniqaInsuranceType, uniqaInsuranceGroup);

                            if(policyInfo != null && policyInfo.AttachmentNumber != 0)
                            {
                                receipt = await this.GetPolicyReceipt(travelInfo.PolicuNo, uniqaInsuranceGroup, uniqaInsuranceType, req.Holder.Name, policyInfo.AttachmentNumber, policyInfo.InstallmentNumber);
                            }
                            
                            var policyPdf = $"{req.MainUserId}\\{travelInfo.PolicuNo}\\policy.pdf";
                            var receiptPdf = $"{req.MainUserId}\\{travelInfo.PolicuNo}\\receipt.pdf";

                            string insuranceId = await this.CreateInsurance(req.StartDate, req.EndDate, insuranceStatus, insuranceCompany.Id.ToString(), travelInfo.Premium,
                                    insuranceType, currency.Id.ToString(), req.EndDate, travelInfo.PolicuNo, 1, policyPdf, 0, receiptPdf);

                            if (string.IsNullOrEmpty(insuranceId))
                            {
                                throw new Exception("Insurance creation failed.");
                            }

                            string insurancePaymentInfoId = await this.CreateInsurancePaymentInformation(insuranceId, paymentInfo.Id);

                            string userInsurancesResult = await this.ManageUsersInsurancesCreation(req.MainUserId, req.Holder, req.Clients, (byte)InsuranceTypeEnum.Travel, insuranceId);

                            if (string.IsNullOrEmpty(userInsurancesResult))
                            {
                                throw new Exception("UserInsurancces creation failed.");
                            }

                            string travelInurance = await this.CreateTravelInsurance(insuranceId, req.TripPurpose, req.Territory, req.Limit);

                            if (string.IsNullOrEmpty(travelInurance))
                            {
                                throw new Exception("Travel insurance creation failed.");
                            }

                            result.PolicyNo = travelInfo.PolicuNo;

                            var savePolicy = this.SaveFile(policyPdf, travelInfo.PolicyPdf);

                            if(receipt.ReceiptPdf != null)
                            {
                                var saveReceipt = this.SaveFile(receiptPdf, receipt.ReceiptPdf);
                            }
                            
                            Task.Run(async () => { this._ftpService.Save(travelInfo.PolicyPdf, travelInfo.PolicuNo); });

                            Task.Run(async () => { await SendInsuredUserEmail(req.Email, (int)insuranceType, req.Holder.Name, travelInfo.PolicuNo, travelInfo.PolicyPdf, null, receipt.ReceiptPdf); });
                            Task.Run(async () => { await SendBrokerEmail((int)insuranceType, travelInfo.PolicuNo, insuranceTypeDescription, travelInfo.PolicyPdf, null, receipt.ReceiptPdf); });

                        }
                        else
                        {
                            result.ErrorId = travelInfo.ErrorId;
                            result.Message = travelInfo.Message;
                        }
                    }
                    else
                    {
                        result.ErrorId = policyResult.ErrorId;
                        result.Message = policyResult.Message;
                        throw new Exception("Uniqa travel policy unsuccess");

                    }
                }
                else
                {
                    result.ErrorId = orderIdResult.ErrorId;
                    result.Message = orderIdResult.Message;
                    throw new Exception("Uniqa travel orderId unsuccess");
                }

                transaction.Commit();

                result.Success = (int)GeneralStatusEnum.Success;
                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                //Task.Run(async () => { this.SendInsuranceErrorEmails(req.Holder.Name, req.Holder.Phone, insuranceTypeDescription); });
                transaction.Rollback();
                return result;

            }
        }

        public async Task<string> CreateTravelInsurance(string insuranceId, byte tripPurpose, byte territory, decimal limit)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);

            var travelInsurance = new TravelInsurance
            {
                Id = Guid.NewGuid(),
                TerritorialValidity = territory,
                TripPurpose = tripPurpose,
                CompensationAmount = limit,
                InsuranceId = currentInsuranceId
            };

            await this._travelInsurancesRepository.AddAsync(travelInsurance);
            await this._travelInsurancesRepository.SaveChangesAsync();

            return travelInsurance.Id.ToString();
        }

        #endregion

        #region Mountain

        public async Task<MountainCalculationResultModel> UniqaMountainCalculation(MountainCalculationInput req)
        {
            var result = new MountainCalculationResultModel();

            try
            {
                var uniqaResponse = await this._uniqaService.MountainCalculation(req);

                if (uniqaResponse != null)
                {
                    result = uniqaResponse;
                    result.CompanyName = GlobalConstants.Uniqa;
                }

                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                return result;

            }
        }

        public async Task<MountainPolicyPayload> UniqaMountainPolicy(MountainPolicyInput req)
        {
            var result = new MountainPolicyPayload();
            var insuranceTypeEnum = InsuranceTypeEnum.Mountain;
            string insuranceTypeDescription = Helpers.GetEnumDescription(insuranceTypeEnum);

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var insuranceCompany = this._insuranceCompanyService.GetCompanyByName<InsuranceCompanyModel>(req.InsuranceCompany);
                var currency = this._currencyService.GetCurrencyByCode<CurrencyModel>(GlobalConstants.Bgn);
                var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.LocalOrderNumber);
                byte insuranceType = (byte)InsuranceTypeEnum.Mountain;
                byte insuranceStatus = (byte)InsuranceStatusEnum.Valid;

                var orderIdResult = await this._uniqaService.MountainOffer(req);

                if (orderIdResult != null && !string.IsNullOrEmpty(orderIdResult.OfferId))
                {
                    var policyResult = await this._uniqaService.IssueMountainPolicyRequest(orderIdResult.OfferId);

                    if (policyResult != null && policyResult.Success)
                    {
                        var mountainInfo = await this._uniqaService.MountainOrderInfo(orderIdResult.OfferId);

                        if (mountainInfo != null && !string.IsNullOrEmpty(mountainInfo.PolicuNo))
                        {
                            string uniqaInsuranceType = GlobalConstants.MountainInsuranceType;
                            bool uniqaInsuranceGroup = GlobalConstants.MountainInsuranceGroup;

                            var receipt = new PayUniqaInstallmentResultModel();
                            var policyInfo = await this._uniqaService.GetPolicyInstallmentInfo(mountainInfo.PolicuNo, uniqaInsuranceType, uniqaInsuranceGroup);

                            if (policyInfo != null && policyInfo.AttachmentNumber != 0)
                            {
                                receipt = await this.GetPolicyReceipt(mountainInfo.PolicuNo, uniqaInsuranceGroup, uniqaInsuranceType, req.Holder.Name, policyInfo.AttachmentNumber, policyInfo.InstallmentNumber);
                            }
                           
                            var policyPdf = $"{req.MainUserId}\\{mountainInfo.PolicuNo}\\policy.pdf";
                            var receiptPdf = $"{req.MainUserId}\\{mountainInfo.PolicuNo}\\receipt.pdf";

                            string insuranceId = await this.CreateInsurance(req.StartDate, req.EndDate, insuranceStatus, insuranceCompany.Id.ToString(), mountainInfo.Premium,
                                    insuranceType, currency.Id.ToString(), req.EndDate, mountainInfo.PolicuNo, 1, policyPdf, 0, receiptPdf);

                            if (string.IsNullOrEmpty(insuranceId))
                            {
                                throw new Exception("Insurance creation failed.");
                            }

                            string insurancePaymentInfoId = await this.CreateInsurancePaymentInformation(insuranceId, paymentInfo.Id);

                            string userInsurancesResult = await this.ManageUsersInsurancesCreation(req.MainUserId, req.Holder, req.Clients, (byte)InsuranceTypeEnum.Mountain, insuranceId);

                            if (string.IsNullOrEmpty(userInsurancesResult))
                            {
                                throw new Exception("UserInsurancces creation failed.");
                            }

                            string travelInurance = await this.CreateMountainInsurance(insuranceId, req.IsExtreme, req.InsuranceSum);

                            if (string.IsNullOrEmpty(travelInurance))
                            {
                                throw new Exception("Mountain insurance creation failed.");
                            }

                            result.PolicyNo = mountainInfo.PolicuNo;

                            var savePolicy = this.SaveFile(policyPdf, mountainInfo.PolicyPdf);

                            if (receipt.ReceiptPdf != null)
                            {
                                var saveReceipt = this.SaveFile(receiptPdf, receipt.ReceiptPdf);
                            }
                                
                            Task.Run(async () => { this._ftpService.Save(mountainInfo.PolicyPdf, mountainInfo.PolicuNo); });

                            Task.Run(async () => { await SendInsuredUserEmail(req.Email, (int)insuranceType, req.Holder.Name, mountainInfo.PolicuNo, mountainInfo.PolicyPdf, null, receipt.ReceiptPdf); });
                            Task.Run(async () => { await SendBrokerEmail((int)insuranceType, mountainInfo.PolicuNo, insuranceTypeDescription, mountainInfo.PolicyPdf, null, receipt.ReceiptPdf); });
                        }
                        else
                        {
                            result.ErrorId = mountainInfo.ErrorId;
                            result.Message = mountainInfo.Message;
                        }
                    }
                    else
                    {
                        result.ErrorId = policyResult.ErrorId;
                        result.Message = policyResult.Message;
                        throw new Exception("Uniqa mountain policy unsuccess");

                    }
                }
                else
                {
                    result.ErrorId = orderIdResult.ErrorId;
                    result.Message = orderIdResult.Message;
                    throw new Exception("Uniqa mountain orderId unsuccess");
                }

                transaction.Commit();
                result.Success = (int)GeneralStatusEnum.Success;
                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                //Task.Run(async () => { this.SendInsuranceErrorEmails(req.Holder.Name, req.Holder.Phone, insuranceTypeDescription); });
                transaction.Rollback();
                return result;

            }
        }

        public async Task<string> CreateMountainInsurance(string insuranceId, bool isExtreme, int insuranceSum)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);

            var mountainInsurance = new MountainInsurance
            {
                Id = Guid.NewGuid(),
                CompensationAmount = insuranceSum,
                InsuranceId = currentInsuranceId,
                IsExtreme = isExtreme
            };

            await this._mountainInsurancesRepository.AddAsync(mountainInsurance);
            await this._mountainInsurancesRepository.SaveChangesAsync();

            return mountainInsurance.Id.ToString();
        }

        #endregion

        #region Health

        public async Task<HealthCalculationResultModel> UniqaHealthCalculation(HealthCalculationInput req)
        {
            var result = new HealthCalculationResultModel();

            try
            {
                var uniqaResponse = await this._uniqaService.HealthCalculation(req);

                if (uniqaResponse != null)
                {
                    result = uniqaResponse;
                    result.CompanyName = GlobalConstants.Uniqa;
                }

                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                return result;

            }
        }
        public async Task<HealthPolicyPayload> UniqaHealthPolicy(HealthPolicyInput req)
        {
            var result = new HealthPolicyPayload();
            var insuranceTypeEnum = InsuranceTypeEnum.Health;
            string insuranceTypeDescription = Helpers.GetEnumDescription(insuranceTypeEnum);

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var insuranceCompany = this._insuranceCompanyService.GetCompanyByName<InsuranceCompanyModel>(req.InsuranceCompany);
                var currency = this._currencyService.GetCurrencyByCode<CurrencyModel>(GlobalConstants.Bgn);
                var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.LocalOrderNumber);
                byte insuranceType = (byte)InsuranceTypeEnum.Health;
                byte insuranceStatus = (byte)InsuranceStatusEnum.Valid;

                var orderIdResult = await this._uniqaService.HealthOffer(req);

                if (orderIdResult != null && !string.IsNullOrEmpty(orderIdResult.OfferId))
                {
                    var policyResult = await this._uniqaService.IssueHealthPolicyRequest(orderIdResult.OfferId);

                    if (policyResult != null && policyResult.Success)
                    {
                        var healthInfo = await this._uniqaService.HealthOrderInfo(orderIdResult.OfferId);

                        if (healthInfo != null && !string.IsNullOrEmpty(healthInfo.PolicuNo))
                        {
                            string uniqaInsuranceType = GlobalConstants.HealthInsuranceType;
                            bool uniqaInsuranceGroup = GlobalConstants.HealthInsuranceGroup;

                            var receipt = new PayUniqaInstallmentResultModel();
                            var policyInfo = await this._uniqaService.GetPolicyInstallmentInfo(healthInfo.PolicuNo, uniqaInsuranceType, uniqaInsuranceGroup);

                            if (policyInfo != null && policyInfo.AttachmentNumber != 0)
                            {
                                receipt = await this.GetPolicyReceipt(healthInfo.PolicuNo, uniqaInsuranceGroup, uniqaInsuranceType, req.Holder.Name, policyInfo.AttachmentNumber, policyInfo.InstallmentNumber);
                            }
                                
                            var policyPdf = $"{req.MainUserId}\\{healthInfo.PolicuNo}\\policy.pdf";
                            var receiptPdf = $"{req.MainUserId}\\{healthInfo.PolicuNo}\\receipt.pdf";

                            var currentEndDate = req.InstallmentCount > 1 ? healthInfo.Installments[1].Date : req.EndDate;

                            int installmentToPay = 0;

                            if (req.InstallmentCount > 1)
                            {
                                installmentToPay = 2;
                            }

                            var currentInstallments = healthInfo.Installments.Take(req.InstallmentCount).ToList();
                            decimal premium = currentInstallments.Sum(x => x.Total + x.Tax);

                            string insuranceId = await this.CreateInsurance(req.StartDate, req.EndDate, insuranceStatus, insuranceCompany.Id.ToString(), premium,
                                    insuranceType, currency.Id.ToString(), currentEndDate, healthInfo.PolicuNo, req.InstallmentCount, policyPdf, installmentToPay, receiptPdf);

                            if (string.IsNullOrEmpty(insuranceId))
                            {
                                throw new Exception("Insurance creation failed.");
                            }

                            string insurancePaymentInfoId = await this.CreateInsurancePaymentInformation(insuranceId, paymentInfo.Id);

                            string userInsurancesResult = await this.ManageUsersInsurancesCreation(req.MainUserId, req.Holder, req.Clients, (byte)InsuranceTypeEnum.Health, insuranceId);

                            if (string.IsNullOrEmpty(userInsurancesResult))
                            {
                                throw new Exception("UserInsurancces creation failed.");
                            }

                            //var installment = healthInfo.Installments.FirstOrDefault();

                            string healthInurance = await this.CreateHealthInsurance(insuranceId, (byte)req.PacketId, 0, req.IsFamily);

                            if (string.IsNullOrEmpty(healthInurance))
                            {
                                throw new Exception("Health insurance creation failed.");
                            }

                            result.PolicyNo = healthInfo.PolicuNo;

                            var savePolicy = this.SaveFile(policyPdf, healthInfo.PolicyPdf);

                            if (receipt.ReceiptPdf != null)
                            {
                                var saveReceipt = this.SaveFile(receiptPdf, receipt.ReceiptPdf);
                            }
                                
                            Task.Run(async () => { this._ftpService.Save(healthInfo.PolicyPdf, healthInfo.PolicuNo); });

                            Task.Run(async () => { await SendInsuredUserEmail(req.Email, (int)insuranceType, req.Holder.Name, healthInfo.PolicuNo, healthInfo.PolicyPdf, null, receipt.ReceiptPdf); });
                            Task.Run(async () => { await SendBrokerEmail((int)insuranceType, healthInfo.PolicuNo, insuranceTypeDescription, healthInfo.PolicyPdf, null, receipt.ReceiptPdf); });
                        }
                        else
                        {
                            result.ErrorId = healthInfo.ErrorId;
                            result.Message = healthInfo.Message;
                        }
                    }
                    else
                    {
                        result.ErrorId = policyResult.ErrorId;
                        result.Message = policyResult.Message;
                        throw new Exception("Uniqa health policy unsuccess");
                    }
                }
                else
                {
                    result.ErrorId = orderIdResult.ErrorId;
                    result.Message = orderIdResult.Message;
                    throw new Exception("Uniqa health orderId unsuccess");
                }

                transaction.Commit();
                result.Success = (int)GeneralStatusEnum.Success;
                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                //Task.Run(async () => { this.SendInsuranceErrorEmails(req.Holder.Name, req.Holder.Phone, insuranceTypeDescription); });
                transaction.Rollback();
                return result;

            }
        }
        public async Task<string> CreateHealthInsurance(string insuranceId, byte packageType, decimal installmentPrice, bool isFamily)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);

            var healthInsurance = new HealthInsurance
            {
                Id = Guid.NewGuid(),
                PackageType = packageType,
                InstallmentPrice = installmentPrice,
                InsuranceId = currentInsuranceId,
                IsFamily = isFamily
            };

            await this._healthInsurancesRepository.AddAsync(healthInsurance);
            await this._healthInsurancesRepository.SaveChangesAsync();

            return healthInsurance.Id.ToString();
        }
        public async Task<HealthInsuranceInstallmentPayload> HealthInsuranceInstallment(HealthInsuranceInstallmentInput req)
        {
            var result = new HealthInsuranceInstallmentPayload();

            var insuranceTypeEnum = InsuranceTypeEnum.Health;
            string insuranceTypeDescription = Helpers.GetEnumDescription(insuranceTypeEnum);

            var insurance = this.GetInsuranceById(req.InsuranceId);
            var mainUserInfo = this.GetInsurerUserInfo(req.UserId, req.InsuranceId);
            var names = mainUserInfo.FirstName + " " + mainUserInfo.LastName;

            if (insurance != null && insurance.Type == (byte)InsuranceTypeEnum.Health)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.LocalOrderNumber);
                int installmentToPay = insurance.InstallmentToPay;
                var insurer = insurance.InsuredUsers.FirstOrDefault(x => x.IsInsurer == true);

                var installmentRequest = new PayUniqaInstallmentRequestModel
                {
                    InstallmentNumber = installmentToPay,
                    ReceiptPayer = insurer.FirstName + " " + insurer.LastName,
                    ReceiptUserCreated = GlobalConstants.BrokerName,
                    PolicyNumber = insurance.PolicyNo

                };

                var payInstallmentResult = await this._uniqaService.PayInstallment(installmentRequest);

                DateTime? currentEndDate = null;

                if (insurance.InstallmentToPay == insurance.InstallmentsNumber)
                {
                    installmentToPay = 0;
                    currentEndDate = insurance.PolicyEndDate;
                }
                else
                {
                    installmentToPay++;
                    //currentEndDate = await this._uniqaService.GetPolicyDueInstallment(insurance.PolicyNo, "Здраве и спокойствие", installmentToPay);
                }

                try
                {
                    if (payInstallmentResult != null && !string.IsNullOrEmpty(payInstallmentResult.ReceiptNumber) && currentEndDate.HasValue)
                    {
                        //Task.Run(async () => { await SendInsuredUserEmail(req.Email, (byte)InsuranceEmailTypeEnum.Installment, insuranceTypeDescription, req.UserId, null, null, payInstallmentResult.ReceiptPdf); });

                        var updateInsurance = await this.UpdateInsurance(insurance.Id, currentEndDate.Value, installmentToPay);

                        if (string.IsNullOrEmpty(updateInsurance))
                        {
                            throw new Exception("Update insurance failed");
                        }

                        var createInsurancePaymentInfo = await this.CreateInsurancePaymentInformation(insurance.Id.ToString(), paymentInfo.Id.ToString());

                        if (string.IsNullOrEmpty(createInsurancePaymentInfo))
                        {
                            throw new Exception("Create InsurancePaymentInformations  failed");
                        }

                        result.Code = (int)GeneralStatusEnum.Success;

                        transaction.Commit();
                    }
                    else
                    {
                        throw new Exception("Health insurance installment error");
                    }
                }
                catch (Exception ex)
                {
                    this.LogError(ex.Message, $"{ex.StackTrace}");
                    _logger.LogError(ex.Message);
                    //Task.Run(async () => { this.SendInsuranceErrorEmails(names, mainUserInfo.PhoneNumber, insuranceTypeDescription, insurance.PolicyNo); });
                    transaction.Rollback();
                }
            }

            return result;
        }

        #endregion

        #region MyThings

        public async Task<MyThingsCalculationResultModel> UniqaMyThingsCalculation(MyThingsCalculationInput req)
        {
            var result = new MyThingsCalculationResultModel();

            try
            {
                var uniqaResponse = await this._uniqaService.MyThingsCalculation(req);

                if (uniqaResponse != null)
                {
                    result = uniqaResponse;
                    result.CompanyName = GlobalConstants.Uniqa;
                }

                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                return result;

            }
        }

        public async Task<MyThingsPolicyPayload> UniqaMyThingsPolicy(MyThingsPolicyInput req)
        {
            var result = new MyThingsPolicyPayload();
            var insuranceTypeEnum = InsuranceTypeEnum.MyThings;
            string insuranceTypeDescription = Helpers.GetEnumDescription(insuranceTypeEnum);

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var insuranceCompany = this._insuranceCompanyService.GetCompanyByName<InsuranceCompanyModel>(req.InsuranceCompany);
                var currency = this._currencyService.GetCurrencyByCode<CurrencyModel>(GlobalConstants.Bgn);
                var paymentInfo = this._paymentService.GetPaymentInfoByOrderNumber<PaymentInfoModel>(req.LocalOrderNumber);
                byte insuranceType = (byte)InsuranceTypeEnum.MyThings;
                byte insuranceStatus = (byte)InsuranceStatusEnum.Valid;

                var orderIdResult = await this._uniqaService.MyThingsOffer(req);

                if (orderIdResult != null && !string.IsNullOrEmpty(orderIdResult.OfferId))
                {
                    var policyResult = await this._uniqaService.IssueMyThingsPolicyRequest(orderIdResult.OfferId);

                    if (policyResult != null && policyResult.Success)
                    {
                        var myThingsInfo = await this._uniqaService.MyThingsOrderInfo(orderIdResult.OfferId);

                        if (myThingsInfo != null && !string.IsNullOrEmpty(myThingsInfo.PolicuNo))
                        {
                            string uniqaInsuranceType = GlobalConstants.MyThingsInsuranceType;
                            bool uniqaInsuranceGroup = GlobalConstants.MyThingsInsuranceGroup;

                            var receipt = new PayUniqaInstallmentResultModel();
                            var policyInfo = await this._uniqaService.GetPolicyInstallmentInfo(myThingsInfo.PolicuNo, uniqaInsuranceType, uniqaInsuranceGroup);

                            if (policyInfo != null && policyInfo.AttachmentNumber != 0)
                            {
                                receipt = await this.GetPolicyReceipt(myThingsInfo.PolicuNo, uniqaInsuranceGroup, uniqaInsuranceType, req.Holder.Name, policyInfo.AttachmentNumber, policyInfo.InstallmentNumber);
                            }
                                
                            var policyPdf = $"{req.MainUserId}\\{myThingsInfo.PolicuNo}\\policy.pdf";
                            var receiptPdf = $"{req.MainUserId}\\{myThingsInfo.PolicuNo}\\receipt.pdf";

                            string insuranceId = await this.CreateInsurance(req.StartDate, req.EndDate, insuranceStatus, insuranceCompany.Id.ToString(), myThingsInfo.Premium,
                                    insuranceType, currency.Id.ToString(), req.EndDate, myThingsInfo.PolicuNo, 1, policyPdf, 0, receiptPdf);

                            if (string.IsNullOrEmpty(insuranceId))
                            {
                                throw new Exception("Insurance creation failed.");
                            }

                            string insurancePaymentInfoId = await this.CreateInsurancePaymentInformation(insuranceId, paymentInfo.Id);

                            string userInsurancesResult = string.Empty;

                            if (req.Holder.UserId != req.MainUserId)
                            {
                                userInsurancesResult = await this.CreateUsersInsurances(req.MainUserId, insuranceId, false, true, false, false);

                                if (!string.IsNullOrEmpty(req.Holder.UserId) && req.Holder.UserId != GlobalConstants.New)
                                {
                                    userInsurancesResult = await this.CreateUsersInsurances(req.Holder.UserId, insuranceId, true, false, false, false);
                                }
                                else
                                {
                                    string userId = await this.AddInsuredUser(req.Holder.Email, req.Holder.Uin, req.Holder.Vat, req.Holder.Phone, req.Holder.Name, null, null);

                                    if (!string.IsNullOrEmpty(userId))
                                    {
                                        userInsurancesResult = await this.CreateUsersInsurances(userId, insuranceId, true, false, false, false);
                                    }
                                }
                            }
                            else
                            {
                                userInsurancesResult = await this.CreateUsersInsurances(req.MainUserId, insuranceId, true, true, false, false);
                            }

                            if (string.IsNullOrEmpty(userInsurancesResult))
                            {
                                throw new Exception("UserInsurancces creation failed.");
                            }

                            string myThingsInuranceId = await this.CreateMyThingsInsurance(insuranceId, (byte)req.PropertyTypeId, (byte)req.ObjectTypeId, req.InsuranceSum, req.Brand, req.Model);

                            if (string.IsNullOrEmpty(myThingsInuranceId))
                            {
                                throw new Exception("MyThings insurance creation failed.");
                            }

                            var questionsResult = await this._questionService.AddQuestions(req.Questionnaire.Questionnaire, myThingsInuranceId);

                            if (questionsResult.Code != (int)GeneralStatusEnum.Success)
                            {
                                throw new Exception("MyThings add questions failed");
                            }

                            result.PolicyNo = myThingsInfo.PolicuNo;

                            var savePolicy = this.SaveFile(policyPdf, myThingsInfo.PolicyPdf);
                            if (receipt.ReceiptPdf != null)
                            {
                                var saveReceipt = this.SaveFile(receiptPdf, receipt.ReceiptPdf);
                            }
                                
                            Task.Run(async () => { this._ftpService.Save(myThingsInfo.PolicyPdf, myThingsInfo.PolicuNo); });

                            Task.Run(async () => { await SendInsuredUserEmail(req.Email, (int)insuranceType, req.Holder.Name, myThingsInfo.PolicuNo, myThingsInfo.PolicyPdf, null, receipt.ReceiptPdf); });
                            Task.Run(async () => { await SendBrokerEmail((int)insuranceType, myThingsInfo.PolicuNo, insuranceTypeDescription, myThingsInfo.PolicyPdf, null, receipt.ReceiptPdf); });
                        }
                        else
                        {
                            result.ErrorId = myThingsInfo.ErrorId;
                            result.Message = myThingsInfo.Message;
                        }
                    }
                    else
                    {
                        result.ErrorId = policyResult.ErrorId;
                        result.Message = policyResult.Message;
                        throw new Exception("Uniqa my things policy unsuccess");
                    }
                }
                else
                {
                    result.ErrorId = orderIdResult.ErrorId;
                    result.Message = orderIdResult.Message;
                    throw new Exception("Uniqa my things orderId unsuccess");
                }

                transaction.Commit();
                result.Success = (int)GeneralStatusEnum.Success;
                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                _logger.LogError(ex.Message);
                //Task.Run(async () => { this.SendInsuranceErrorEmails(req.Holder.Name, req.Holder.Phone, insuranceTypeDescription); });
                transaction.Rollback();
                return result;

            }
        }

        public async Task<string> CreateMyThingsInsurance(string insuranceId, byte propertyType, byte objectType, decimal insuranceSum, string brand, string model)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);

            var myThingsInsurance = new MyThingsInsurance
            {
                Id = Guid.NewGuid(),
                PropertyType = propertyType,
                ObjectType = objectType,
                InsuranceSum = insuranceSum,
                InsuranceId = currentInsuranceId,
                Brand = brand,
                Model = model
            };

            await this._myThingsInsurancesRepository.AddAsync(myThingsInsurance);
            await this._myThingsInsurancesRepository.SaveChangesAsync();

            return myThingsInsurance.Id.ToString();
        }

        #endregion

        #region Common

        public async Task<string> CreateInsurance(DateTime startDate, DateTime endDate, byte status, string insuranceCompanyId,
             decimal price, byte type, string currencyId, DateTime currentEndDate, string policyNo, int installments,
             string pdfUrl, int installmentToPay, string receiptUrl)
        {
            var currentInsuranceCompanyId = Helpers.ParseStringToGuid(insuranceCompanyId);
            var currentCurrencyId = Helpers.ParseStringToGuid(currencyId);

            var insurance = new Insurance
            {
                Id = Guid.NewGuid(),
                PolicyEndDate = endDate,
                StartDate = startDate,
                Status = status,
                InsuranceCompanyId = currentInsuranceCompanyId,
                CreationDate = DateTime.Now,
                Price = price,
                Type = type,
                CurrencyId = currentCurrencyId,
                CurrentEndDate = currentEndDate,
                PolicyNo = policyNo,
                InstallmentsNumber = installments,
                InstallmentToPay = installmentToPay,
                PdfUrl = pdfUrl,
                ReceiptUrl = receiptUrl
            };

            await this._insuranceRepository.AddAsync(insurance);
            await this._insuranceRepository.SaveChangesAsync();

            return insurance.Id.ToString();
        }

        public InsuranceModel GetInsuranceById(string id)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(id);

            var insurance = this._insuranceRepository
                .AllAsNoTracking()
                .Where(x => x.Id == currentInsuranceId)
                .Select(x => new InsuranceModel
                {
                    Id = x.Id.ToString(),
                    StartDate = x.StartDate,
                    PolicyEndDate = x.PolicyEndDate,
                    CurrentEndDate = x.CurrentEndDate,
                    CreationDate = x.CreationDate,
                    Status = x.Status,
                    Price = x.Price,
                    CurrencyCode = x.Currency.Code,
                    Type = x.Type,
                    PolicyNo = x.PolicyNo,
                    InstallmentsNumber = x.InstallmentsNumber,
                    PdfUrl = x.PdfUrl,
                    InstallmentToPay = x.InstallmentToPay,
                    InsuranceCompany = new InsuranceCompanyModel
                    {
                        Id = x.InsuranceCompanyId.ToString(),
                        CompanyName = x.InsuranceCompany.CompanyName
                    },
                    Vehicle = x.CivilInsurance == null ? null : new VehicleResultModel
                    {
                        Id = x.CivilInsurance.VehicleId.ToString(),
                        RegistrationCertificateNumber = x.CivilInsurance.Vehicle.RegistrationCertificateNumber,
                        //VehiclePlateNumber = x.CivilInsurance.Vehicle.VehiclePlateNumber,
                        UserId = x.CivilInsurance.Vehicle.UserId.ToString(),
                        //VehicleModelId = x.CivilInsurance.Vehicle.VehicleModelId.ToString(),
                        //VehicleTypeId = x.CivilInsurance.Vehicle.VehicleTypeId.ToString(),
                        //VehicleUsageId = x.CivilInsurance.Vehicle.VehicleUsageId.ToString(),
                        //VehicleModel = x.CivilInsurance.Vehicle.VehicleModel.Name,
                        //VehicleBrandId = x.CivilInsurance.Vehicle.VehicleModel.VehicleBrandId.ToString(),
                        //VehicleBrand = x.CivilInsurance.Vehicle.VehicleModel.VehicleBrand.Name,
                        //VehicleType = x.CivilInsurance.Vehicle.VehicleType.Name,
                        //VehicleUsage = x.CivilInsurance.Vehicle.VehicleUsage.Name,
                        FirstRegistrationDate = x.CivilInsurance.Vehicle.FirstRegistrationDate
                    },
                    CivilInsurance = x.CivilInsurance == null ? null : new CivilInsuranceModel
                    {
                        Id = x.CivilInsurance.Id.ToString(),
                        FirstInstallmentPrice = x.CivilInsurance.FirstInstallmentPrice,
                        SecondInstallmentPrice = x.CivilInsurance.SecondInstallmentPrice,
                        ThirdInstallmentPrice = x.CivilInsurance.ThirdInstallmentPrice,
                        FourthInstallmentPrice = x.CivilInsurance.FourthInstallmentPrice,
                        SecondInstallmentTax = x.CivilInsurance.SecondInstallmentTax,
                        ThirdInstallmentTax = x.CivilInsurance.SecondInstallmentTax,
                        FourthInstallmentTax = x.CivilInsurance.FourthInstallmentTax,
                        GreenCardNo = x.CivilInsurance.GreenCard.GreenCardNumber,
                        StickerNo = x.CivilInsurance.Sticker.StickerNumber,
                        InsuranceId = x.CivilInsurance.InsuranceId.ToString(),
                        SecondInstallmentDate = x.CivilInsurance.SecondInstallmentDate.HasValue ? x.CivilInsurance.SecondInstallmentDate.Value : null,
                        ThirdInstallmentDate = x.CivilInsurance.ThirdInstallmentDate.HasValue ? x.CivilInsurance.ThirdInstallmentDate.Value : null,
                        FourthInstallmentDate = x.CivilInsurance.FourthInstallmentDate.HasValue ? x.CivilInsurance.FourthInstallmentDate.Value : null,
                        GreenCardPdfUrl = x.CivilInsurance.GreenCardPdfUrl
                    },
                    HealthInsurance = x.HealthInsurance == null ? null : new HealthInsuranceModel
                    {
                        Id = x.HealthInsurance.Id.ToString(),
                        PackageType = x.HealthInsurance.PackageType,
                        InstallmentPrice = x.HealthInsurance.InstallmentPrice,
                        InsuranceId = x.HealthInsurance.InsuranceId.ToString()
                    },
                    MountainInsurance = x.MountainInsurance == null ? null : new MountainInsuranceModel
                    {
                        Id = x.MountainInsurance.Id.ToString(),
                        CompensationAmount = x.MountainInsurance.CompensationAmount,
                        IsExtreme = x.MountainInsurance.IsExtreme,
                        InsuranceId = x.MountainInsurance.InsuranceId.ToString()
                    },
                    TravelInsurance = x.TravelInsurance == null ? null : new TravelInsuranceModel
                    {
                        Id = x.TravelInsurance.Id.ToString(),
                        TerritorialValidity = x.TravelInsurance.TerritorialValidity,
                        TravelPurpose = x.TravelInsurance.TripPurpose,
                        CompensationAmount = x.TravelInsurance.CompensationAmount,
                        InsuranceId = x.TravelInsurance.InsuranceId.ToString()
                    },
                    InsuredUsers = x.UsersInsurances.Select(y => new InsuredUserModel
                    {
                        IsInsurer = y.IsInsurer,
                        IsMainUser = y.IsMainUser,
                        IsUsualDriver = y.IsUsualDriver,
                        FirstName = y.User.FirstName,
                        SurName = y.User.SurName,
                        LastName = y.User.LastName,
                        UserId = y.UserId.ToString(),
                        VatNumber = y.User.VatNumber,
                        UiNumber = y.User.UiNumber,
                        CompanyName = y.User.CompanyName
                    }).ToList()
                })
                .FirstOrDefault();

            return insurance;
        }
        public List<InsuranceModel> GetAllUserInsurances(string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var insurances = this._insuranceRepository
                .AllAsNoTracking()
                //.Where(x => x.UsersInsurances.Any(y => y.UserId == currentUserId && y.IsMainUser == true))
                .Where(x => x.UsersInsurances.Any(y => y.UserId == currentUserId))
                .Select(x => new InsuranceModel
                {
                    Id = x.Id.ToString(),
                    StartDate = x.StartDate,
                    PolicyEndDate = x.PolicyEndDate,
                    CurrentEndDate = x.CurrentEndDate,
                    CreationDate = x.CreationDate,
                    Status = x.Status,
                    Price = x.Price,
                    CurrencyCode = x.Currency.Code,
                    Type = x.Type,
                    PolicyNo = x.PolicyNo,
                    InstallmentsNumber = x.InstallmentsNumber,
                    PdfUrl = x.PdfUrl,
                    InstallmentToPay = x.InstallmentToPay,
                    InsuranceCompany = new InsuranceCompanyModel
                    {
                        Id = x.InsuranceCompanyId.ToString(),
                        CompanyName = x.InsuranceCompany.CompanyName
                    },
                    Vehicle = x.CivilInsurance == null ? null : new VehicleResultModel
                    {
                        Id = x.CivilInsurance.VehicleId.ToString(),
                        RegistrationCertificateNumber = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.RegistrationCertificateNumber : string.Empty,
                        PlateNumber = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.PlateNumber : x.CivilInsurance.VehiclePlateNumber,
                        UserId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.UserId.ToString() : currentUserId.ToString(),
                        ModelId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.ModelId : 0,
                        VehicleTypeId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleTypeId : 1,
                        VehicleUsageId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleUsageId : 1,
                        Model = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.Model : x.CivilInsurance.VehicleModel,
                        BrandId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.BrandId : 1,
                        Brand = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.Brand : x.CivilInsurance.VehicleBrand,
                        VehicleType = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleType : string.Empty,
                        VehicleUsage = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleUsage : string.Empty,
                        FirstRegistrationDate = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.FirstRegistrationDate : DateTime.Now
                    },
                    CivilInsurance = x.CivilInsurance == null ? null : new CivilInsuranceModel
                    {
                        Id = x.CivilInsurance.Id.ToString(),
                        FirstInstallmentPrice = x.CivilInsurance.FirstInstallmentPrice,
                        SecondInstallmentPrice = x.CivilInsurance.SecondInstallmentPrice,
                        ThirdInstallmentPrice = x.CivilInsurance.ThirdInstallmentPrice,
                        FourthInstallmentPrice = x.CivilInsurance.FourthInstallmentPrice,
                        SecondInstallmentTax = x.CivilInsurance.SecondInstallmentTax,
                        ThirdInstallmentTax = x.CivilInsurance.SecondInstallmentTax,
                        FourthInstallmentTax = x.CivilInsurance.FourthInstallmentTax,
                        GreenCardNo = x.CivilInsurance.GreenCard.GreenCardNumber,
                        StickerNo = x.CivilInsurance.Sticker.StickerNumber,
                        InsuranceId = x.CivilInsurance.InsuranceId.ToString(),
                        SecondInstallmentDate = x.CivilInsurance.SecondInstallmentDate.HasValue ? x.CivilInsurance.SecondInstallmentDate.Value : null,
                        ThirdInstallmentDate = x.CivilInsurance.ThirdInstallmentDate.HasValue ? x.CivilInsurance.ThirdInstallmentDate.Value : null,
                        FourthInstallmentDate = x.CivilInsurance.FourthInstallmentDate.HasValue ? x.CivilInsurance.FourthInstallmentDate.Value : null,
                        GreenCardPdfUrl = x.CivilInsurance.GreenCardPdfUrl
                    },
                    HealthInsurance = x.HealthInsurance == null ? null : new HealthInsuranceModel
                    {
                        Id = x.HealthInsurance.Id.ToString(),
                        PackageType = x.HealthInsurance.PackageType,
                        InstallmentPrice = x.HealthInsurance.InstallmentPrice,
                        InsuranceId = x.HealthInsurance.InsuranceId.ToString(),
                        IsFamily = x.HealthInsurance.IsFamily
                    },
                    MountainInsurance = x.MountainInsurance == null ? null : new MountainInsuranceModel
                    {
                        Id = x.MountainInsurance.Id.ToString(),
                        CompensationAmount = x.MountainInsurance.CompensationAmount,
                        IsExtreme = x.MountainInsurance.IsExtreme,
                        InsuranceId = x.MountainInsurance.InsuranceId.ToString()
                    },
                    TravelInsurance = x.TravelInsurance == null ? null : new TravelInsuranceModel
                    {
                        Id = x.TravelInsurance.Id.ToString(),
                        TerritorialValidity = x.TravelInsurance.TerritorialValidity,
                        TravelPurpose = x.TravelInsurance.TripPurpose,
                        CompensationAmount = x.TravelInsurance.CompensationAmount,
                        InsuranceId = x.TravelInsurance.InsuranceId.ToString()
                    },
                    MyThingsInsurance = x.MyThingsInsurance == null ? null : new MyThingsInsuranceModel
                    {
                        Id = x.MyThingsInsurance.Id.ToString(),
                        PropertyType = x.MyThingsInsurance.PropertyType,
                        ObjectType = x.MyThingsInsurance.ObjectType,
                        InsuranceSum = x.MyThingsInsurance.InsuranceSum,
                        Brand = x.MyThingsInsurance.Brand,
                        Model = x.MyThingsInsurance.Model,
                        InsuranceId = x.MyThingsInsurance.InsuranceId.ToString()
                    },
                    InsuredUsers = x.UsersInsurances.Select(y => new InsuredUserModel
                    {
                        IsInsurer = y.IsInsurer,
                        IsMainUser = y.IsMainUser,
                        IsUsualDriver = y.IsUsualDriver,
                        FirstName = y.User.FirstName,
                        SurName = y.User.SurName,
                        LastName = y.User.LastName,
                        UserId = y.UserId.ToString(),
                        VatNumber = y.User.VatNumber,
                        UiNumber = y.User.UiNumber,
                        CompanyName = y.User.CompanyName,
                        BirthDate = y.User.BirthDate,
                        IsInsured = y.IsInsured

                    }).ToList()
                })
                .ToList();


            return insurances;
        }
        public AllInsurancesPayload GetAllInsurances(AllInsurancesInput input)
        {
            var result = new AllInsurancesPayload();
            int toSkip  = (input.Page - 1) * input.PerPage;

            DateTime startDate = input.StartDate.HasValue ? input.StartDate.Value : (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            DateTime endDate = input.EndDate.HasValue ? input.EndDate.Value : DateTime.Now;

            var insurancesCount = this.GetAllInsurancesCountFiltered(startDate, endDate);
            var totalPages = (int)Math.Ceiling((double)insurancesCount / input.PerPage);

            var insurances = this._insuranceRepository
                .AllAsNoTracking()
                .Where(x => x.CreationDate >= startDate && x.CreationDate <= endDate)
                .OrderBy(x => x.CreationDate) // Order by CreationDate before pagination
                .Skip(toSkip)
                .Take(input.PerPage)
                .Select(x => new InsuranceModel
                {
                    Id = x.Id.ToString(),
                    StartDate = x.StartDate,
                    PolicyEndDate = x.PolicyEndDate,
                    CurrentEndDate = x.CurrentEndDate,
                    CreationDate = x.CreationDate,
                    Status = x.Status,
                    Price = x.Price,
                    CurrencyCode = x.Currency.Code,
                    Type = x.Type,
                    PolicyNo = x.PolicyNo,
                    InstallmentsNumber = x.InstallmentsNumber,
                    PdfUrl = x.PdfUrl,
                    InstallmentToPay = x.InstallmentToPay,
                    InsuranceCompany = new InsuranceCompanyModel
                    {
                        Id = x.InsuranceCompanyId.ToString(),
                        CompanyName = x.InsuranceCompany.CompanyName
                    },
                    Vehicle = x.CivilInsurance == null ? null : new VehicleResultModel
                    {
                        Id = x.CivilInsurance.VehicleId.ToString(),
                        RegistrationCertificateNumber = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.RegistrationCertificateNumber : string.Empty,
                        PlateNumber = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.PlateNumber : x.CivilInsurance.VehiclePlateNumber,
                        UserId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.UserId.ToString() : string.Empty,
                        ModelId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.ModelId : 0,
                        VehicleTypeId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleTypeId : 1,
                        VehicleUsageId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleUsageId : 1,
                        Model = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.Model : x.CivilInsurance.VehicleModel,
                        BrandId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.BrandId : 1,
                        Brand = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.Brand : x.CivilInsurance.VehicleBrand,
                        VehicleType = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleType : string.Empty,
                        VehicleUsage = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleUsage : string.Empty,
                        FirstRegistrationDate = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.FirstRegistrationDate : DateTime.Now
                    },
                    CivilInsurance = x.CivilInsurance == null ? null : new CivilInsuranceModel
                    {
                        Id = x.CivilInsurance.Id.ToString(),
                        FirstInstallmentPrice = x.CivilInsurance.FirstInstallmentPrice,
                        SecondInstallmentPrice = x.CivilInsurance.SecondInstallmentPrice,
                        ThirdInstallmentPrice = x.CivilInsurance.ThirdInstallmentPrice,
                        FourthInstallmentPrice = x.CivilInsurance.FourthInstallmentPrice,
                        SecondInstallmentTax = x.CivilInsurance.SecondInstallmentTax,
                        ThirdInstallmentTax = x.CivilInsurance.SecondInstallmentTax,
                        FourthInstallmentTax = x.CivilInsurance.FourthInstallmentTax,
                        GreenCardNo = x.CivilInsurance.GreenCard.GreenCardNumber,
                        StickerNo = x.CivilInsurance.Sticker.StickerNumber,
                        InsuranceId = x.CivilInsurance.InsuranceId.ToString(),
                        SecondInstallmentDate = x.CivilInsurance.SecondInstallmentDate.HasValue ? x.CivilInsurance.SecondInstallmentDate.Value : null,
                        ThirdInstallmentDate = x.CivilInsurance.ThirdInstallmentDate.HasValue ? x.CivilInsurance.ThirdInstallmentDate.Value : null,
                        FourthInstallmentDate = x.CivilInsurance.FourthInstallmentDate.HasValue ? x.CivilInsurance.FourthInstallmentDate.Value : null,
                        GreenCardPdfUrl = x.CivilInsurance.GreenCardPdfUrl
                    },
                    HealthInsurance = x.HealthInsurance == null ? null : new HealthInsuranceModel
                    {
                        Id = x.HealthInsurance.Id.ToString(),
                        PackageType = x.HealthInsurance.PackageType,
                        InstallmentPrice = x.HealthInsurance.InstallmentPrice,
                        InsuranceId = x.HealthInsurance.InsuranceId.ToString(),
                        IsFamily = x.HealthInsurance.IsFamily
                    },
                    MountainInsurance = x.MountainInsurance == null ? null : new MountainInsuranceModel
                    {
                        Id = x.MountainInsurance.Id.ToString(),
                        CompensationAmount = x.MountainInsurance.CompensationAmount,
                        IsExtreme = x.MountainInsurance.IsExtreme,
                        InsuranceId = x.MountainInsurance.InsuranceId.ToString()
                    },
                    TravelInsurance = x.TravelInsurance == null ? null : new TravelInsuranceModel
                    {
                        Id = x.TravelInsurance.Id.ToString(),
                        TerritorialValidity = x.TravelInsurance.TerritorialValidity,
                        TravelPurpose = x.TravelInsurance.TripPurpose,
                        CompensationAmount = x.TravelInsurance.CompensationAmount,
                        InsuranceId = x.TravelInsurance.InsuranceId.ToString()
                    },
                    MyThingsInsurance = x.MyThingsInsurance == null ? null : new MyThingsInsuranceModel
                    {
                        Id = x.MyThingsInsurance.Id.ToString(),
                        PropertyType = x.MyThingsInsurance.PropertyType,
                        ObjectType = x.MyThingsInsurance.ObjectType,
                        InsuranceSum = x.MyThingsInsurance.InsuranceSum,
                        Brand = x.MyThingsInsurance.Brand,
                        Model = x.MyThingsInsurance.Model,
                        InsuranceId = x.MyThingsInsurance.InsuranceId.ToString()
                    },
                    InsuredUsers = x.UsersInsurances.Select(y => new InsuredUserModel
                    {
                        IsInsurer = y.IsInsurer,
                        IsMainUser = y.IsMainUser,
                        IsUsualDriver = y.IsUsualDriver,
                        FirstName = y.User.FirstName,
                        SurName = y.User.SurName,
                        LastName = y.User.LastName,
                        UserId = y.UserId.ToString(),
                        VatNumber = y.User.VatNumber,
                        UiNumber = y.User.UiNumber,
                        CompanyName = y.User.CompanyName,
                        BirthDate = y.User.BirthDate,
                        IsInsured = y.IsInsured,
                        IsDeleted = y.User.IsDeleted.HasValue ? y.User.IsDeleted.Value : false

                    }).ToList()
                })
                .OrderBy(x => x.CreationDate)
                .ToList();

            result.Insurances = insurances;
            result.TotalPages = totalPages;

            return result;
        }
        public AllInsurancesSumUpModel GetAllInsurancesSumUp()
        {
            var insurances = this._insuranceRepository
                     .AllAsNoTracking()
                     .Select(x => new InsuranceModel
                     {
                         CurrencyCode = x.Currency.Code,
                         Price = x.Price,
                         InsuredUsers = x.UsersInsurances.Select(y => new InsuredUserModel
                         {
                             IsInsurer = y.IsInsurer,
                             UserId = y.UserId.ToString()
                         }).ToList(),
                         InsuranceCompany = new InsuranceCompanyModel
                         {
                             Id = x.InsuranceCompanyId.ToString(),
                             CompanyName = x.InsuranceCompany.CompanyName
                         }
                     }).ToList();

            var result = new AllInsurancesSumUpModel();

            var totalBgnPrice = insurances.Where(x => x.CurrencyCode.Equals("BGN")).Sum(i => i.Price);
            var totalEurPrice = insurances.Where(x => x.CurrencyCode.Equals("Eur")).Sum(i => i.Price);

            decimal exchangeRateEurToBgn = GlobalConstants.EurToBgn;
            decimal totalBgnPriceConverted = (totalEurPrice * exchangeRateEurToBgn) + totalBgnPrice;

            result.TotalInsurancesPrice = totalBgnPriceConverted;
            result.TotalInsurances = insurances.Count;

            var insurersCount = insurances
                .SelectMany(insurance => insurance.InsuredUsers)
                .Where(user => user.IsInsurer)
                .GroupBy(user => user.UserId)
                .Count();

            result.TotalInsurers = insurersCount;

            var insurancesCounts = insurances
                .GroupBy(insurance => insurance.InsuranceCompany?.CompanyName)
                .Select(group => new { CompanyName = group.Key, Count = group.Count() })
                .ToList();

            var insurancesSum = insurances
                .GroupBy(insurance => insurance.InsuranceCompany?.CompanyName)
                .Select(group =>
                {
                    return new { CompanyName = group.Key, TotalPrice = group.Sum(i => i.Price) };
                })
                .ToList();

            result.InsuranceChartData = new InsuranceChartData
            {
                DecimalTotals = insurancesSum.Select(x => x.TotalPrice).ToList(),
                Total = insurancesCounts.Select(x => x.Count).ToList(),
                Labels = insurancesCounts.Select(x => x.CompanyName).ToList(),
            };

            return result;
        }
        public int GetAllInsurancesCount()
        {
            var insurances = this._insuranceRepository
                .AllAsNoTracking()
                .Count();

            return insurances;
        }
        public int GetInsurancesCountDateRange(DateTime startDate, DateTime endDate)
        {
            var insurances = this._insuranceRepository
                .AllAsNoTracking()
                .Where(x => x.StartDate >= startDate && x.StartDate <= endDate).OrderBy(x => x.StartDate)
                .Count();

            return insurances;
        }
        public async Task<List<InsuranceModel>> GetInsurancesForPage(InsurancesForPageInput insurancesForPageInput)
        {
            int insurancesToSkip = (insurancesForPageInput.Page - 1) * insurancesForPageInput.InsurancesPerPage;

            if (!insurancesForPageInput.StartDate.HasValue || !insurancesForPageInput.EndDate.HasValue)
            {
                insurancesForPageInput.StartDate = DateTime.Now.AddDays(-7);
                insurancesForPageInput.EndDate = DateTime.Now;
            }

            var insurancesForPage = this._insuranceRepository
                .AllAsNoTracking()
                .Where(x => x.CreationDate >= insurancesForPageInput.StartDate.Value && x.CreationDate <= insurancesForPageInput.EndDate.Value).OrderBy(x => x.StartDate)
                .Skip(insurancesToSkip)
                .Take(30)
                .Select(x => new InsuranceModel
                {
                    Id = x.Id.ToString(),
                    StartDate = x.StartDate,
                    PolicyEndDate = x.PolicyEndDate,
                    CurrentEndDate = x.CurrentEndDate,
                    CreationDate = x.CreationDate,
                    Status = x.Status,
                    Price = x.Price,
                    CurrencyCode = x.Currency.Code,
                    Type = x.Type,
                    PolicyNo = x.PolicyNo,
                    InstallmentsNumber = x.InstallmentsNumber,
                    PdfUrl = x.PdfUrl,
                    InstallmentToPay = x.InstallmentToPay,
                    InsuranceCompany = new InsuranceCompanyModel
                    {
                        Id = x.InsuranceCompanyId.ToString(),
                        CompanyName = x.InsuranceCompany.CompanyName
                    },
                    Vehicle = x.CivilInsurance == null ? null : new VehicleResultModel
                    {
                        Id = x.CivilInsurance.VehicleId.ToString(),
                        RegistrationCertificateNumber = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.RegistrationCertificateNumber : string.Empty,
                        PlateNumber = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.PlateNumber : x.CivilInsurance.VehiclePlateNumber,
                        UserId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.UserId.ToString() : string.Empty,
                        ModelId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.ModelId : 0,
                        VehicleTypeId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleTypeId : 1,
                        VehicleUsageId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleUsageId : 1,
                        Model = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.Model : x.CivilInsurance.VehicleModel,
                        BrandId = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.BrandId : 1,
                        Brand = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.Brand : x.CivilInsurance.VehicleBrand,
                        VehicleType = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleType : string.Empty,
                        VehicleUsage = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.VehicleUsage : string.Empty,
                        FirstRegistrationDate = x.CivilInsurance.Vehicle != null ? x.CivilInsurance.Vehicle.FirstRegistrationDate : DateTime.Now
                    },
                    CivilInsurance = x.CivilInsurance == null ? null : new CivilInsuranceModel
                    {
                        Id = x.CivilInsurance.Id.ToString(),
                        FirstInstallmentPrice = x.CivilInsurance.FirstInstallmentPrice,
                        SecondInstallmentPrice = x.CivilInsurance.SecondInstallmentPrice,
                        ThirdInstallmentPrice = x.CivilInsurance.ThirdInstallmentPrice,
                        FourthInstallmentPrice = x.CivilInsurance.FourthInstallmentPrice,
                        SecondInstallmentTax = x.CivilInsurance.SecondInstallmentTax,
                        ThirdInstallmentTax = x.CivilInsurance.SecondInstallmentTax,
                        FourthInstallmentTax = x.CivilInsurance.FourthInstallmentTax,
                        GreenCardNo = x.CivilInsurance.GreenCard.GreenCardNumber,
                        StickerNo = x.CivilInsurance.Sticker.StickerNumber,
                        InsuranceId = x.CivilInsurance.InsuranceId.ToString(),
                        SecondInstallmentDate = x.CivilInsurance.SecondInstallmentDate.HasValue ? x.CivilInsurance.SecondInstallmentDate.Value : null,
                        ThirdInstallmentDate = x.CivilInsurance.ThirdInstallmentDate.HasValue ? x.CivilInsurance.ThirdInstallmentDate.Value : null,
                        FourthInstallmentDate = x.CivilInsurance.FourthInstallmentDate.HasValue ? x.CivilInsurance.FourthInstallmentDate.Value : null,
                        GreenCardPdfUrl = x.CivilInsurance.GreenCardPdfUrl
                    },
                    HealthInsurance = x.HealthInsurance == null ? null : new HealthInsuranceModel
                    {
                        Id = x.HealthInsurance.Id.ToString(),
                        PackageType = x.HealthInsurance.PackageType,
                        InstallmentPrice = x.HealthInsurance.InstallmentPrice,
                        InsuranceId = x.HealthInsurance.InsuranceId.ToString(),
                        IsFamily = x.HealthInsurance.IsFamily
                    },
                    MountainInsurance = x.MountainInsurance == null ? null : new MountainInsuranceModel
                    {
                        Id = x.MountainInsurance.Id.ToString(),
                        CompensationAmount = x.MountainInsurance.CompensationAmount,
                        IsExtreme = x.MountainInsurance.IsExtreme,
                        InsuranceId = x.MountainInsurance.InsuranceId.ToString()
                    },
                    TravelInsurance = x.TravelInsurance == null ? null : new TravelInsuranceModel
                    {
                        Id = x.TravelInsurance.Id.ToString(),
                        TerritorialValidity = x.TravelInsurance.TerritorialValidity,
                        TravelPurpose = x.TravelInsurance.TripPurpose,
                        CompensationAmount = x.TravelInsurance.CompensationAmount,
                        InsuranceId = x.TravelInsurance.InsuranceId.ToString()
                    },
                    MyThingsInsurance = x.MyThingsInsurance == null ? null : new MyThingsInsuranceModel
                    {
                        Id = x.MyThingsInsurance.Id.ToString(),
                        PropertyType = x.MyThingsInsurance.PropertyType,
                        ObjectType = x.MyThingsInsurance.ObjectType,
                        InsuranceSum = x.MyThingsInsurance.InsuranceSum,
                        Brand = x.MyThingsInsurance.Brand,
                        Model = x.MyThingsInsurance.Model,
                        InsuranceId = x.MyThingsInsurance.InsuranceId.ToString()
                    },
                    InsuredUsers = x.UsersInsurances.Select(y => new InsuredUserModel
                    {
                        IsInsurer = y.IsInsurer,
                        IsMainUser = y.IsMainUser,
                        IsUsualDriver = y.IsUsualDriver,
                        FirstName = y.User.FirstName,
                        SurName = y.User.SurName,
                        LastName = y.User.LastName,
                        UserId = y.UserId.ToString(),
                        VatNumber = y.User.VatNumber,
                        UiNumber = y.User.UiNumber,
                        CompanyName = y.User.CompanyName,
                        BirthDate = y.User.BirthDate,
                        IsInsured = y.IsInsured,
                        IsDeleted = y.User.IsDeleted.HasValue ? y.User.IsDeleted.Value : false

                    }).ToList()
                })
                .OrderBy(i => i.CreationDate)
                .ToList();

            return insurancesForPage;
        }
        public async Task<string> CreateInsurancePaymentInformation(string insuranceId, string paymentInformationId)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);
            var currentPaymentInformationId = Helpers.ParseStringToGuid(paymentInformationId);

            var insurancePaymentInfo = new InsurancesPaymentInformation
            {
                Id = Guid.NewGuid(),
                InsuranceId = currentInsuranceId,
                PaymentInformationId = currentPaymentInformationId
            };

            await this._insurancePaymentInfoRepository.AddAsync(insurancePaymentInfo);
            await this._insurancePaymentInfoRepository.SaveChangesAsync();

            return insurancePaymentInfo.Id.ToString();
        }
        public async Task<string> UpdateInsurance(string insuranceId, DateTime currentEndDate, int installmentToPay)
        {
            string result = string.Empty;
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);

            var insurance = this._insuranceRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == currentInsuranceId);

            if (insurance != null)
            {
                insurance.CurrentEndDate = currentEndDate;
                insurance.InstallmentToPay = installmentToPay;

                this._insuranceRepository.Update(insurance);
                await this._insuranceRepository.SaveChangesAsync();

                result = insurance.Id.ToString();
            }

            return result;
        }
        public async Task<string> CreateUsersInsurances(string userId, string insuranceId, bool isInsurer, bool isMainUser, bool isUsualDriver, bool isInsured)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);

            var userInsurances = new UsersInsurance
            {
                Id = Guid.NewGuid(),
                UserId = currentUserId,
                IsInsurer = isInsurer,
                IsMainUser = isMainUser,
                IsUsualDriver = isUsualDriver,
                InsuranceId = currentInsuranceId,
                IsInsured = isInsured
            };

            await this._usersInsurancesRepository.AddAsync(userInsurances);
            await this._usersInsurancesRepository.SaveChangesAsync();

            return userInsurances.Id.ToString();
        }
        public byte[] GetInsurancePolicy(string userId, string insuranceId)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var policyUrl = this._insuranceRepository
                .AllAsNoTracking()
                .Include(x => x.UsersInsurances)
                .Where(x => x.Id == currentInsuranceId && x.UsersInsurances.Any(y => y.UserId == currentUserId))
                .Select(x => x.PdfUrl)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(policyUrl))
            {
                return this._readWriteService.GetFile(policyUrl);
            }

            return null;
        }
        public byte[] GetInsuranceReceipt(string userId, string insuranceId)
        {
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var receiptUrl = this._insuranceRepository
                .AllAsNoTracking()
                .Include(x => x.UsersInsurances)
                .Where(x => x.Id == currentInsuranceId && x.UsersInsurances.Any(y => y.UserId == currentUserId))
                .Select(x => x.ReceiptUrl)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(receiptUrl))
            {
                return this._readWriteService.GetFile(receiptUrl);
            }

            return null;
        }
        public async Task<SendEmailCascoRequestPayload> SendEmailCascoRequest(CascoRequestEmailInput req)
        {
            var result = new SendEmailCascoRequestPayload
            {
                Code = (int)GeneralStatusEnum.Unsuccess,
                Message = GeneralStatusEnum.Unsuccess.ToString()
            };

            try
            {
                var isSent = SendCascoRequestEmail(req);

                if (isSent)
                {
                    result.Code = (int)GeneralStatusEnum.Success;
                    result.Message = GeneralStatusEnum.Success.ToString();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                this._logger.LogError(ex, "Error sending casco request email.");
            }

            return result;
        }
        private int SaveFile(string url)
        {
            var file = this.GetDummyFile();
            int result = this._readWriteService.SaveFile(url, file);

            return result;
        }
        public int SaveFile(string url, byte[] file)
        {
            int result = this._readWriteService.SaveFile(url, file);

            return result;
        }
        private byte[] GetDummyFile()
        {
            var file = this._readWriteService.GetFile("dummy.pdf");

            return file;
        }
        private async Task<bool> SendInsuredUserEmail(string emailTo, int insuranceType, string name, string policyNo, byte[] policy = null, byte[] greenCard = null, byte[] receipt = null, string insuranceCompany = null)
        {
            var emailAttachments = this.PopulateAttachments(policy, greenCard, receipt, policyNo);
            string emailBody = string.Empty;
            string subject = string.Empty;

            int emailType = (int)EmailTypeEnum.Client;
            var emailBodyResult = this.GetEmailBody(insuranceType, emailType);

            emailBody = emailBodyResult.EmailBody;
            subject = emailBodyResult.EmailSubject;

            var nameArr = name.Split(" ").ToArray();
            string firstName = nameArr.Length > 0 ? nameArr[0] : string.Empty;

            if(insuranceType == (int)InsuranceTypeEnum.Civil)
            {
                string infoDoc = this.GetCivilInsuranceInfoDocument(insuranceCompany);
                emailBody = emailBody.Replace(GlobalConstants.InfoDocPattern, infoDoc);
            }

            emailBody = emailBody.Replace(GlobalConstants.PolicyNoPattern, policyNo);
            emailBody = emailBody.Replace(GlobalConstants.FirstNamePattern, firstName);
            subject = subject.Replace(GlobalConstants.PolicyNoPattern, policyNo);

            var mailData = new MailDataWithAttachment();
            mailData.EmailSubject = subject;
            mailData.EmailBody = emailBody;
            mailData.EmailToId = emailTo;
            mailData.EmailToName = emailTo;
            mailData.EmailAttachments = emailAttachments;

            var result = _mailService.SendMailWithAttachment(mailData);

            return result;
        }
        private async Task<bool> SendBrokerEmail(int insuranceType, string policyNo, string insuranceTypeDesc, byte[] policy = null, byte[] greenCard = null, byte[] receipt = null)
        {
            var emailAttachments = this.PopulateAttachments(policy, greenCard, receipt, policyNo);
            string emailBody = string.Empty;
            string subject = string.Empty;

            int emailType = (int)EmailTypeEnum.Broker;
            var emailBodyResult = this.GetEmailBody(insuranceType, emailType);

            emailBody = emailBodyResult.EmailBody;
            subject = emailBodyResult.EmailSubject;

            emailBody = emailBody.Replace(GlobalConstants.PolicyNoPattern, policyNo);
            emailBody = emailBody.Replace(GlobalConstants.InsuranceTypePattern, insuranceTypeDesc);
            subject = subject.Replace(GlobalConstants.PolicyNoPattern, policyNo);

            var mailData = new MailDataWithAttachment();
            mailData.EmailSubject = subject;
            mailData.EmailBody = emailBody;
            mailData.EmailAttachments = emailAttachments;

            var result = _mailService.SendMailWithAttachment(mailData);

            return result;
        }
        private List<AttachmentModel> PopulateAttachments(byte[] policy, byte[] greenCard, byte[] receipt, string policyNo)
        {
            var result = new List<AttachmentModel>();

            if (policy != null)
            {
                var policyAttachment = new AttachmentModel()
                {
                    FileName = $"{policyNo}.pdf",
                    ContentType = "application/pdf",
                    ByteArray = policy
                };
                result.Add(policyAttachment);
            }

            if (greenCard != null)
            {
                var greenCardAttachment = new AttachmentModel()
                {
                    FileName = $"{policyNo}-greenCard.pdf",
                    ContentType = "application/pdf",
                    ByteArray = greenCard
                };

                result.Add(greenCardAttachment);
            }

            if (receipt != null)
            {
                var receiptAttachment = new AttachmentModel()
                {
                    FileName = $"{policyNo}-receipt.pdf",
                    ContentType = "application/pdf",
                    ByteArray = receipt
                };

                result.Add(receiptAttachment);
            }


            return result;
        }
        private UserModel GetInsurerUserInfo(string userId, string insuranceId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);
            var currentInsuranceId = Helpers.ParseStringToGuid(insuranceId);

            var userInfo = this._usersInsurancesRepository
                .AllAsNoTracking()
                .Include(x => x.Insurance)
                .Include(x => x.User)
                .Where(x => x.UserId == currentUserId && x.InsuranceId == currentInsuranceId)
                .Select(x => new UserModel
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    UiNumber = x.User.UiNumber,
                    VatNumber = x.User.VatNumber,
                    CompanyName = x.User.CompanyName
                })
                .FirstOrDefault();

            return userInfo;
        }
        private async Task SendInsuranceErrorEmails(string names, string phone, string insuranceType, string policyNo = null)
        {
            this._errorService.SendInsuranceErrorEmail(names, phone, policyNo, insuranceType);
        }
        private bool SendCascoRequestEmail(CascoRequestEmailInput req)
        {
            string email = GlobalConstants.CascoRequestEmаil;
            string subject = GlobalConstants.CascoRequestEmailHeader;

            string drivingExperienceText = req.UserInfo.DrivingExperience.HasValue ? (req.UserInfo.DrivingExperience.Value == 11 ? "над 10 години" : $"{req.UserInfo.DrivingExperience.Value} год") : "няма";
            string engineVolumeOrKilowatts = req.VehicleInfo.EngineType == 5 ? $"{req.VehicleInfo.BatteryCapacity} kW/h" : $"{req.VehicleInfo.EngineVolume} куб.см";

            email = email.Replace(GlobalConstants.FirstNamePattern, req.UserInfo.FirstName);
            email = email.Replace(GlobalConstants.SurNamePattern, req.UserInfo.SurName);
            email = email.Replace(GlobalConstants.LastNamePattern, req.UserInfo.LastName);
            email = email.Replace(GlobalConstants.PhonePattern, req.UserInfo.Phone);
            email = email.Replace(GlobalConstants.EmailPattern, req.UserInfo.Email);
            email = email.Replace(GlobalConstants.DrivingExperiencePattern, drivingExperienceText);
            email = email.Replace(GlobalConstants.UinPattern, $"{req.UserInfo.Uin}");
            email = email.Replace(GlobalConstants.BrandPattern, req.VehicleInfo.VehicleBrand);
            email = email.Replace(GlobalConstants.ModelPattern, req.VehicleInfo.VehicleModel);
            email = email.Replace(GlobalConstants.YearPattern, req.VehicleInfo.FirstRegistrationDate);
            email = email.Replace(GlobalConstants.EngineTypePattern, req.VehicleInfo.EngineTypeText);
            email = email.Replace(GlobalConstants.VehicleUsagePattern, req.VehicleInfo.VehicleUsage);
            email = email.Replace(GlobalConstants.VehicleKilowatts, $"{req.VehicleInfo.VehicleKilowatts} kW");
            email = email.Replace(GlobalConstants.VolumeOrBatteryCapacity, engineVolumeOrKilowatts.ToString());

            var mailData = new MailData();
            mailData.EmailSubject = subject;
            mailData.EmailBody = email;

            var result = _mailService.SendMail(mailData);

            return result;
        }
        private async Task AddInsuredUsersToUsersInsurances(List<InsuredClientDataModel> users, string insuranceId, byte insuranceType)
        {
            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.UserId) && user.UserId != GlobalConstants.New)
                {
                    var currentUserInsurancesResult = await this.CreateUsersInsurances(user.UserId, insuranceId, false, false, false, true);
                }
                else
                {
                    string names = user.FirstName + " " + user.LastName;
                    string userId = string.Empty;

                    if (insuranceType == (byte)InsuranceTypeEnum.Mountain || insuranceType == (byte)InsuranceTypeEnum.Health)
                    {
                        userId = await this.AddInsuredUser(null, user.Uin, null, null, names, null, user.BirthDate);
                    }
                    else if (insuranceType == (byte)InsuranceTypeEnum.Travel)
                    {
                        userId = await this.AddInsuredUser(null, user.Uin, null, null, null, names, user.BirthDate);
                    }


                    if (!string.IsNullOrEmpty(userId))
                    {
                        var currentUserInsurancesResult = await this.CreateUsersInsurances(userId, insuranceId, false, false, false, true);
                    }
                }

            }
        }
        private Task<string> AddInsuredUser(string email, string uin, string vatNumber, string phoneNumber, string names, string latinNames, DateTime? birthDate)
        {
            string firstName = null;
            string surName = null;
            string lastName = null;

            string latinFirstName = null;
            string latinSurName = null;
            string latinLastName = null;

            string companyName = null;

            if (!string.IsNullOrEmpty(vatNumber))
            {
                companyName = string.IsNullOrEmpty(names) ? latinNames : names;
            }
            else
            {
                if (!string.IsNullOrEmpty(names))
                {
                    var nameArr = names.Split(" ").ToArray();
                    firstName = nameArr[0];
                    surName = nameArr.Length > 2 ? nameArr[1] : null;
                    lastName = nameArr.Length > 2 ? nameArr[2] : nameArr[1];
                }
                else
                {
                    var latinNameArr = latinNames.Split(" ").ToArray();

                    latinFirstName = latinNameArr[0];
                    latinSurName = latinNameArr.Length > 2 ? latinNameArr[1] : null;
                    latinLastName = latinNameArr.Length > 2 ? latinNameArr[2] : latinNameArr[1];
                }

            }

            var req = new UserModel
            {
                FirstName = firstName,
                SurName = surName,
                LastName = lastName,
                LatinFirstName = latinFirstName,
                LatinSurName = latinSurName,
                LatinLastName = latinLastName,
                Email = email,
                UiNumber = uin,
                VatNumber = vatNumber,
                CompanyName = companyName,
                PhoneNumber = phoneNumber,
                BirthDate = birthDate.HasValue ? birthDate.Value : null,
            };

            var result = this._userService.AddUser(req);
            return result;
        }
        private void LogError(string exMessage, string trace)
        {
            string message = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm")} {trace} - {exMessage}";
            string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy")}.txt";

            this._errorService.LogError(message, fileName);
        }
        private async void LogInsuranceError(string requestJson, string responseJson, string idNumber)
        {
            string header = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm")} id:{idNumber}";
            string request = $"Request - {requestJson}";
            string response = $"Response - {responseJson}";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);
            sb.AppendLine(request);
            sb.AppendLine(response);

            string message = sb.ToString().TrimEnd();
            string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy")}.txt";

            this._errorService.LogError(message, fileName);
        }
        private async Task<PayUniqaInstallmentResultModel> GetPolicyReceipt(string policyNo, bool insuranceGroup, string insuranceType, string insurerNames, int attachmentNumber = 0, int installmentNumber = 1)
        {
            var installmentReq = new PayUniqaInstallmentRequestModel
            {
                InstallmentNumber = installmentNumber,
                ReceiptPayer = insurerNames,
                ReceiptUserCreated = GlobalConstants.BrokerName,
                PolicyNumber = policyNo,
                InsuranceGroup = insuranceGroup,
                InsuranceType = insuranceType,
                AttachmentNumber = attachmentNumber
            };

            var receiptInfo = await this._uniqaService.PayInstallment(installmentReq);

            return receiptInfo;
        }
        private MailData GetEmailBody(int insuranceType, int emailType)
        {
            var mailData = new MailData();

            string emailBody = string.Empty;
            string path = string.Empty;
            string emailSubject = string.Empty;

            if (emailType == (int)EmailTypeEnum.Broker)
            {
                if (insuranceType == (int)InsuranceTypeEnum.Civil)
                {
                    path = this._configuration.GetSection("ReadWriteOptions").GetSection("SirmaEmailBroker").Value;
                }
                else
                {
                    path = this._configuration.GetSection("ReadWriteOptions").GetSection("UniqaEmailBroker").Value;

                    switch (insuranceType)
                    {
                        case (int)InsuranceTypeEnum.Civil:
                            emailSubject = GlobalConstants.CivilInsuranceHeaderBroker;
                            break;

                        case (int)InsuranceTypeEnum.Travel:
                            emailSubject = GlobalConstants.TravelInsuranceHeaderBroker;
                            break;

                        case (int)InsuranceTypeEnum.Mountain:
                            emailSubject = GlobalConstants.MountainInsuranceHeaderBroker;
                            break;

                        case (int)InsuranceTypeEnum.Health:
                            emailSubject = GlobalConstants.HealthInsuranceHeaderBroker;
                            break;

                        case (int)InsuranceTypeEnum.MyThings:
                            emailSubject = GlobalConstants.MyThingsInsuranceHeaderBroker;
                            break;
                    }
                }
            }
            else
            {
                switch (insuranceType)
                {
                    case (int)InsuranceTypeEnum.Civil:
                        path = this._configuration.GetSection("ReadWriteOptions").GetSection("ClientEmailGO").Value;
                        emailSubject = GlobalConstants.CivilInsuranceHeaderClient;
                        break;

                    case (int)InsuranceTypeEnum.Travel:
                        path = this._configuration.GetSection("ReadWriteOptions").GetSection("ClientEmailMyTravel").Value;
                        emailSubject = GlobalConstants.TravelInsuranceHeaderClient;
                        break;

                    case (int)InsuranceTypeEnum.Mountain:
                        path = this._configuration.GetSection("ReadWriteOptions").GetSection("ClientEmailMountain").Value;
                        emailSubject = GlobalConstants.MountainInsuranceHeaderClient;
                        break;

                    case (int)InsuranceTypeEnum.Health:
                        path = this._configuration.GetSection("ReadWriteOptions").GetSection("ClientEmailHealth").Value;
                        emailSubject = GlobalConstants.HealthInsuranceHeaderClient;
                        break;

                    case (int)InsuranceTypeEnum.MyThings:
                        path = this._configuration.GetSection("ReadWriteOptions").GetSection("ClientEmailMyThings").Value;
                        emailSubject = GlobalConstants.MyThingsInsuranceHeaderClient;
                        break;
                }
            }

            emailBody = this._readWriteService.ReadFile(path);

            mailData.EmailBody = emailBody;
            mailData.EmailSubject = emailSubject;

            return mailData;
        }

        private string GetCivilInsuranceInfoDocument(string insuranceCompany)
        {
            string result = string.Empty;

            switch (insuranceCompany)
            {
                case GlobalConstants.Dzi: result = GlobalConstants.DziCivilInfo; break;

                case GlobalConstants.Euroins: result = GlobalConstants.EuroinsCivilInfo; break;

                case GlobalConstants.Generali: result = GlobalConstants.GeneraliCivilInfo; break;

                case GlobalConstants.Groupama: result = GlobalConstants.GroupamaCivilInfo; break;

                case GlobalConstants.Ozk: result = GlobalConstants.OzkCivilInfo; break;

                case GlobalConstants.Bulins: result = GlobalConstants.BulinsCivilInfo; break;

                case GlobalConstants.Bulstrad: result = GlobalConstants.BulstradCivilInfo; break;

                case GlobalConstants.Uniqa: result = GlobalConstants.UniqaCivilInfo; break;
                case GlobalConstants.Levins: result = GlobalConstants.LevinsCivilInfo; break;

                default: break;                    
            }

            return result;
        }

        private int GetAllInsurancesCountFiltered(DateTime startDate, DateTime endDate)
        {
            var insurances = this._insuranceRepository
                .AllAsNoTracking()
                .Where(x => x.CreationDate >= startDate && x.CreationDate <= endDate)
                .Count();

            return insurances;
        }

        private async Task<string> ManageUsersInsurancesCreation(string mainUserId, InsurerCustomerModel holder, List<InsuredClientDataModel> clients,
            byte insuranceType, string insuranceId)
        {
            string userInsurancesResult = string.Empty;
            var insuredUsers = new List<InsuredClientDataModel>();

            if (holder.UserId != mainUserId)
            {              
                userInsurancesResult = await this.CreateUsersInsurances(mainUserId, insuranceId, false, true, false, false);

                if (clients.Any(x => x.Uin == holder.Uin))
                {
                    if (!string.IsNullOrEmpty(holder.UserId) && holder.UserId != GlobalConstants.New)
                    {
                        userInsurancesResult = await this.CreateUsersInsurances(holder.UserId, insuranceId, true, false, false, true);
                        insuredUsers = clients.Where(x => x.Uin != holder.Uin).ToList();
                    }
                    else
                    {
                        string userId = await this.AddInsuredUser(holder.Email, holder.Uin, holder.Vat, holder.Phone, holder.Name, null, null);

                        if (!string.IsNullOrEmpty(userId))
                        {
                            userInsurancesResult = await this.CreateUsersInsurances(userId, insuranceId, true, false, false, false);
                        }

                        insuredUsers = clients;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(holder.UserId) && holder.UserId != GlobalConstants.New)
                    {
                        userInsurancesResult = await this.CreateUsersInsurances(holder.UserId, insuranceId, true, false, false, false);

                    }
                    else
                    {
                        string userId = await this.AddInsuredUser(holder.Email, holder.Uin, holder.Vat, holder.Phone, holder.Name, null, null);

                        if (!string.IsNullOrEmpty(userId))
                        {
                            userInsurancesResult = await this.CreateUsersInsurances(userId, insuranceId, true, false, false, false);
                        }
                    }

                    insuredUsers = clients;
                }
            }
            else
            {
                if (clients.Any(x => x.Uin == holder.Uin))
                {
                    userInsurancesResult = await this.CreateUsersInsurances(mainUserId, insuranceId, true, true, false, true);
                    insuredUsers = clients.Where(x => x.Uin != holder.Uin).ToList();
                }
                else
                {
                    userInsurancesResult = await this.CreateUsersInsurances(mainUserId, insuranceId, true, true, false, false);
                    insuredUsers = clients;
                }
            }

            await this.AddInsuredUsersToUsersInsurances(insuredUsers, insuranceId, insuranceType);
            return userInsurancesResult;
        }

        #endregion
    }
}
