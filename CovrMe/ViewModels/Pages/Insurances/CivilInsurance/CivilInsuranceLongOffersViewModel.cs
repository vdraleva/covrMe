using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.CivilInsurance.Request;
using CovrMe.Models.Insurances.Request.CivilInsurances;
using CovrMe.Models.Insurances.Result.CivilInsurances;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceLongOffersViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private int _installment = 1;
        private int _oneInstallment = 1;
        private int _twoInstallments = 2;
        private int _fourInstallments = 4;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;
        private List<string> _availableOffersCompanyNames;

        private string _vehicle;

        //collections
        private ObservableCollection<CivilInsuranceSearchModel> _offersCollection;
        private ObservableCollection<CivilInsuranceSearchModel> _offersOneInstallmentCollection;
        private ObservableCollection<CivilInsuranceSearchModel> _offersTwoInstallmentsCollection;
        private ObservableCollection<CivilInsuranceSearchModel> _offersFourInstallmentsCollection;
        private ObservableCollection<CivilInsuranceSearchModel> _offersShortCollection;

        //insurance
        private bool _isOnePayment;
        private bool _isTwoPayment;
        private bool _isFourPayment;
        private string _onePayBtnColor;
        private string _twoPayBtnColor;
        private string _fourPayBtnColor;
        private CivilInsuranceSearchModel _selectedOffer;


        //services
        private IInsuranceService _insuranceService;

        #endregion

        public CivilInsuranceLongOffersViewModel(IInsuranceService insuranceService)
        {
            IsOnePaymentsVisible = true;
            IsTwoPaymentsVisible = false;
            IsFourPaymentsVisible = false;

            this._insuranceService = insuranceService;

            this.OffersCollection = new ObservableCollection<CivilInsuranceSearchModel>();
            this.OffersOneInstallmentCollection = new ObservableCollection<CivilInsuranceSearchModel>();
            this.OffersTwoInstallmentsCollection = new ObservableCollection<CivilInsuranceSearchModel>();
            this.OffersFourInstallmentsCollection = new ObservableCollection<CivilInsuranceSearchModel>();
            this.OffersShortCollection = new ObservableCollection<CivilInsuranceSearchModel>();
            OnePayBtnColor = GlobalConstants.ActiveColorPurple;
            TwoPayBtnColor = GlobalConstants.WhiteColor;
            FourPayBtnColor = GlobalConstants.WhiteColor;
        }

        #region Collections

        public ObservableCollection<CivilInsuranceSearchModel> OffersCollection
        {
            get => _offersCollection;
            set => SetProperty(ref _offersCollection, value);
        }
        public ObservableCollection<CivilInsuranceSearchModel> OffersOneInstallmentCollection
        {
            get => _offersOneInstallmentCollection;
            set => SetProperty(ref _offersOneInstallmentCollection, value);
        }
        public ObservableCollection<CivilInsuranceSearchModel> OffersTwoInstallmentsCollection
        {
            get => _offersTwoInstallmentsCollection;
            set => SetProperty(ref _offersTwoInstallmentsCollection, value);
        }
        public ObservableCollection<CivilInsuranceSearchModel> OffersFourInstallmentsCollection
        {
            get => _offersFourInstallmentsCollection;
            set => SetProperty(ref _offersFourInstallmentsCollection, value);
        }
        public ObservableCollection<CivilInsuranceSearchModel> OffersShortCollection
        {
            get => _offersShortCollection;
            set => SetProperty(ref _offersShortCollection, value);
        }
        #endregion

        #region Props

        public CivilInsuranceSearchModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }
        public int Installment
        {
            get { return _installment; }
            set { SetProperty(ref _installment, value); }
        }
        public bool IsOnePaymentsVisible
        {
            get { return _isOnePayment; }
            set { SetProperty(ref _isOnePayment, value); }
        }
        public bool IsTwoPaymentsVisible
        {
            get { return _isTwoPayment; }
            set { SetProperty(ref _isTwoPayment, value); }
        }
        public bool IsFourPaymentsVisible
        {
            get { return _isFourPayment; }
            set { SetProperty(ref _isFourPayment, value); }
        }
        public string OnePayBtnColor
        {
            get { return _onePayBtnColor; }
            set { SetProperty(ref _onePayBtnColor, value); }
        }
        public string TwoPayBtnColor
        {
            get { return _twoPayBtnColor; }
            set { SetProperty(ref _twoPayBtnColor, value); }
        }
        public string FourPayBtnColor
        {
            get { return _fourPayBtnColor; }
            set { SetProperty(ref _fourPayBtnColor, value); }
        }
        public string Vehicle
        {
            get { return _vehicle; }
            set { SetProperty(ref _vehicle, value); }
        }

        #endregion

        #region Commands
        [RelayCommand]
        private async void Continue(CivilInsuranceSearchModel selectedOffer)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                if (selectedOffer == null)
                {
                    return;
                }

                this.SelectedOffer = selectedOffer;
                ShowLoading();

                decimal price = this.SelectedOffer.Installments.Any() ? this.SelectedOffer.Installments[0].Total : this.SelectedOffer.Premium;

                var offer = new InsuranceOfferModel
                {
                    CompanyName = this.SelectedOffer.InsuranceCompanyName,
                    TotalPrice = this.SelectedOffer.Premium,
                    Price = price,
                    PriceFormatted = price.ToString() + " лв.",
                    InsuranceType = (byte)InsuranceTypeEnum.Civil,
                    Installment = this.Installment,
                    CompanyLogo = this.SelectedOffer.LogoSrc,
                    Installments = this.SelectedOffer.Installments,
                    LongSearch = true
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"selectedOffer", offer},
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},
                    };
                await Navigation.PushAsync<CivilInsuranceCalendarPage>(parameters);
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }
        #endregion

        #region Methods
        public async Task BoxToggle(string toggleName)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;
                ShowLoading();
                switch (toggleName)
                {
                    case "one":
                        IsOnePaymentsVisible = true;
                        IsTwoPaymentsVisible = false;
                        IsFourPaymentsVisible = false;

                        OnePayBtnColor = GlobalConstants.ActiveColorPurple;
                        TwoPayBtnColor = GlobalConstants.WhiteColor;
                        FourPayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 1;

                        UpdateMainList(_oneInstallment);
                        await GetOffersShort(_oneInstallment);

                        break;

                    case "two":
                        IsTwoPaymentsVisible = true;
                        IsOnePaymentsVisible = false;
                        IsFourPaymentsVisible = false;

                        TwoPayBtnColor = GlobalConstants.ActiveColorPurple;
                        OnePayBtnColor = GlobalConstants.WhiteColor;
                        FourPayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 2;

                        UpdateMainList(_twoInstallments);
                        await GetOffersShort(_twoInstallments);

                        break;

                    case "four":
                        IsFourPaymentsVisible = true;
                        IsOnePaymentsVisible = false;
                        IsTwoPaymentsVisible = false;

                        FourPayBtnColor = GlobalConstants.ActiveColorPurple;
                        TwoPayBtnColor = GlobalConstants.WhiteColor;
                        OnePayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 4;

                        UpdateMainList(_fourInstallments);
                        await GetOffersShort(_fourInstallments);

                        break;

                    default:
                        IsOnePaymentsVisible = true;
                        IsTwoPaymentsVisible = false;
                        IsFourPaymentsVisible = false;

                        OnePayBtnColor = GlobalConstants.ActiveColorPurple;
                        TwoPayBtnColor = GlobalConstants.WhiteColor;
                        FourPayBtnColor = GlobalConstants.WhiteColor;
                        break;
                }
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }

        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this._ownerUserInfo = query.FirstOrDefault(x => x.Key == "ownerUserInfo").Value as InsuranceUserInfoModel;
            this._usualUserInfo = query.FirstOrDefault(x => x.Key == "usualUserInfo").Value as InsuranceUserInfoModel;
            this._vehicleInfo = query.FirstOrDefault(x => x.Key == "userVehicleInfo").Value as InsuranceVehicleInfo;
            this._availableOffersCompanyNames = query.FirstOrDefault(x => x.Key == "availableOffersCompanyNames").Value as List<string>;

            var offersShortCollection = new ObservableCollection<CivilInsuranceSearchModel>();
            offersShortCollection = query.FirstOrDefault(x => x.Key == "oneInstallmentOffersCollection").Value as ObservableCollection<CivilInsuranceSearchModel>;
            this.OffersShortCollection.Clear();
            this.OffersShortCollection = offersShortCollection;

            this.Vehicle = this._vehicleInfo.VehicleBrand + " " + this._vehicleInfo.VehicleModel;
            Task.Run(async () => { await GetOffersLong(); }).Wait();
        }

        private async Task GetOffersLong(int? installment = null)
        {
            var httpClient = HttpClientFactory.Create();
            var req = new CivilInsuranceLongSearchInput()
            {
                OwnerExperience = this._ownerUserInfo.DrivingExperiance,
                OwnerUiNumber = this._ownerUserInfo.Uin,
                OwnerVatNumber = this._ownerUserInfo.VatNumber,
                VehiclePlateNumber = this._vehicleInfo.PlateNumber,
                VehicleUsage = this._vehicleInfo.VehicleUsage,
                RegistrationCertificateNumber = this._vehicleInfo.RegistrationCertificateNumber,
                OwnerPostalCode = this._ownerUserInfo.PostalCode,
                VehicleType = this._vehicleInfo.VehicleTypeId,
                OwnerCity = this._ownerUserInfo.CityName,
                OwnerRegion = this._ownerUserInfo.RegionName,
                OwnerMunicipality = this._ownerUserInfo.MunicipalityName,
                OwnerCountryCode = this._ownerUserInfo.CountryCode,
                OwnerName = string.IsNullOrEmpty(this._ownerUserInfo.VatNumber) ? (this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.SurName + " " + this._ownerUserInfo.LastName) : this._ownerUserInfo.CompanyName,
                OwnerAddress = this._ownerUserInfo.Address,
                Installments = null,
                OwnerBirthdate = this._ownerUserInfo.BirthDateString,
                VehicleFirstReg = this._vehicleInfo.FirstRegistrationDate,
                VehicleBrand = this._vehicleInfo.VehicleBrand,
                VehicleModel = this._vehicleInfo.VehicleModel,
                UsualExperience = this._usualUserInfo != null ? this._usualUserInfo.DrivingExperiance : 0,
                UsualUiNumber = this._usualUserInfo != null ? this._usualUserInfo.Uin : string.Empty,
                UsualPostalCode = this._usualUserInfo != null ? this._usualUserInfo.PostalCode : string.Empty,
                UsualRegion = this._usualUserInfo != null ? this._usualUserInfo.RegionName : string.Empty,
                UsualMunicipality = this._usualUserInfo != null ? this._usualUserInfo.MunicipalityName : string.Empty,
                UsualCity = this._usualUserInfo != null ? this._usualUserInfo.CityName : string.Empty,
                UsualName = this._usualUserInfo != null ? this._usualUserInfo.FirstName + " " + this._usualUserInfo.SurName + " " + this._usualUserInfo.LastName : string.Empty,
                UsualAddress = this._usualUserInfo != null ? this._usualUserInfo.Address : string.Empty,
                UsualBirthdate = this._usualUserInfo != null ? this._usualUserInfo.BirthDateString : string.Empty,
                UsualCountryCode = this._usualUserInfo != null ? this._usualUserInfo.CountryCode : string.Empty,
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
                GrossWeight = this._vehicleInfo.GrossWeight,
                NetWeight = this._vehicleInfo.NetWeight,
                InsuranceCompanies = _availableOffersCompanyNames
            };

            var jwt = App.JwtToken;
            var offersResponse = await this._insuranceService.CivilInsuranceLongSearch(req, httpClient, jwt);

            foreach (var offer in offersResponse.InsuranceOffers)
            {
                if (offer.Success == (int)GeneralStatusEnum.Success)
                {
                    var current = new CivilInsuranceSearchModel
                    {
                        InsuranceCompanyName = offer.InsuranceCompanyName,
                        Premium = offer.CivilInsuranceOffer.CivilInsuranceCalculation.PremiumWithTax,
                        PremiumFormatted = offer.CivilInsuranceOffer.CivilInsuranceCalculation.PremiumWithTax.ToString() + " лв.",
                        LogoSrc = Helpers.GetImageSrc(offer.InsuranceCompanyName)
                    };

                    if (offer.CivilInsuranceOffer?.CivilInsuranceCalculation != null)
                    {
                        //if (offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments != null)
                        //{
                        //    AddInstallments(current, offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments);
                        //}

                        if (offer.CivilInsuranceOffer.CivilInsuranceCalculation.FullInstallmentsBreakdown != null)
                        {
                            AddFullInstallments(current, offer.CivilInsuranceOffer.CivilInsuranceCalculation.FullInstallmentsBreakdown);
                        }
                    }
                    OffersOneInstallmentCollection.Add(current);
                }
            }
            this.OffersCollection.Clear();
            this.OffersCollection = new ObservableCollection<CivilInsuranceSearchModel>(OffersOneInstallmentCollection);
        }
        private async Task GetOffersShort(int installment)
        {
            var offersShortCollection = new ObservableCollection<CivilInsuranceSearchModel>();

            var httpClient = HttpClientFactory.Create();
            var req = new CivilInsuranceSearchInput()
            {
                OwnerExperience = this._ownerUserInfo.DrivingExperiance,
                OwnerUiNumber = this._ownerUserInfo.Uin,
                OwnerVatNumber = this._ownerUserInfo.VatNumber,
                VehiclePlateNumber = this._vehicleInfo.PlateNumber,
                VehicleUsage = this._vehicleInfo.VehicleUsage,
                RegistrationCertificateNumber = this._vehicleInfo.RegistrationCertificateNumber,
                OwnerPostalCode = this._ownerUserInfo.PostalCode,
                VehicleType = this._vehicleInfo.VehicleTypeId,
                OwnerCity = this._ownerUserInfo.CityName,
                OwnerRegion = this._ownerUserInfo.RegionName,
                OwnerMunicipality = this._ownerUserInfo.MunicipalityName,
                OwnerCountryCode = this._ownerUserInfo.CountryCode,
                OwnerName = string.IsNullOrEmpty(this._ownerUserInfo.VatNumber) ? (this._ownerUserInfo.FirstName + " " + this._ownerUserInfo.SurName + " " + this._ownerUserInfo.LastName) : this._ownerUserInfo.CompanyName,
                OwnerAddress = this._ownerUserInfo.Address,
                Installments = installment,
                OwnerBirthdate = this._ownerUserInfo.BirthDateString,
                VehicleFirstReg = this._vehicleInfo.FirstRegistrationDate,
                VehicleBrand = this._vehicleInfo.VehicleBrand,
                VehicleModel = this._vehicleInfo.VehicleModel,
                UsualExperience = this._usualUserInfo != null ? this._usualUserInfo.DrivingExperiance : 0,
                UsualUiNumber = this._usualUserInfo != null ? this._usualUserInfo.Uin : string.Empty,
                UsualPostalCode = this._usualUserInfo != null ? this._usualUserInfo.PostalCode : string.Empty,
                UsualRegion = this._usualUserInfo != null ? this._usualUserInfo.RegionName : string.Empty,
                UsualMunicipality = this._usualUserInfo != null ? this._usualUserInfo.MunicipalityName : string.Empty,
                UsualName = this._usualUserInfo != null ? this._usualUserInfo.FirstName + " " + this._usualUserInfo.SurName + " " + this._usualUserInfo.LastName : string.Empty,
                UsualAddress = this._usualUserInfo != null ? this._usualUserInfo.Address : string.Empty,
                UsualBirthdate = this._usualUserInfo != null ? this._usualUserInfo.BirthDateString : string.Empty,
                UsualCountryCode = this._usualUserInfo != null ? this._usualUserInfo.CountryCode : string.Empty,
                UsualCity = this._usualUserInfo != null ? this._usualUserInfo.CityName : string.Empty,
            };

            var jwt = App.JwtToken;
            var offersResponse = await this._insuranceService.CivilInsuranceSearch(req, httpClient, jwt);

            foreach (var offer in offersResponse.InsuranceOffers)
            {
                if (offer.Success == (int)GeneralStatusEnum.Success)
                {
                    var current = new CivilInsuranceSearchModel
                    {
                        InsuranceCompanyName = offer.InsuranceCompanyName,
                        Premium = offer.CivilInsuranceOffer.CivilInsuranceCalculation.PremiumWithTax,
                        PremiumFormatted = offer.CivilInsuranceOffer.CivilInsuranceCalculation.PremiumWithTax.ToString() + " лв.",
                        LogoSrc = Helpers.GetImageSrc(offer.InsuranceCompanyName)
                    };

                    if (offer.CivilInsuranceOffer != null)
                    {
                        if (offer.CivilInsuranceOffer.CivilInsuranceCalculation != null)
                        {
                            if (offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments != null)
                            {
                                AddInstallments(current, offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments);
                            }
                        }
                    }

                    offersShortCollection.Add(current);
                }
            }

            this.OffersShortCollection.Clear();
            this.OffersShortCollection = offersShortCollection;
        }

        private CivilInsuranceSearchModel CreateNewSearchModel(CivilInsuranceSearchModel source)
        {
            return new CivilInsuranceSearchModel
            {
                InsuranceCompanyName = source.InsuranceCompanyName,
                Premium = source.Premium,
                PremiumFormatted = source.PremiumFormatted,
                LogoSrc = source.LogoSrc,
                Installments = new ObservableCollection<InstallmentModel>()
            };
        }
        private void AddFullInstallments(CivilInsuranceSearchModel current, CivilInsuranceFullInstallments fullInstallments)
        {
            if (fullInstallments.FirstInstallment != null)
            {
                AddInstallments(current, fullInstallments.FirstInstallment);
            }

            if (fullInstallments.SecondInstallment != null)
            {
                CivilInsuranceSearchModel secondInstallmentModel = CreateNewSearchModel(current);
                AddInstallmentToCollection(fullInstallments.SecondInstallment.FirstInstallment, secondInstallmentModel, 1, 0);
                AddInstallmentToCollection(fullInstallments.SecondInstallment.SecondInstallment, secondInstallmentModel, 2, 1);
                OffersTwoInstallmentsCollection.Add(secondInstallmentModel);
            }

            if (fullInstallments.FourthInstallment != null)
            {
                CivilInsuranceSearchModel fourthInstallmentModel = CreateNewSearchModel(current);
                AddInstallmentToCollection(fullInstallments.FourthInstallment.FirstInstallment, fourthInstallmentModel, 1, 0);
                AddInstallmentToCollection(fullInstallments.FourthInstallment.SecondInstallment, fourthInstallmentModel, 2, 1);
                AddInstallmentToCollection(fullInstallments.FourthInstallment.ThirdInstallment, fourthInstallmentModel, 3, 2);
                AddInstallmentToCollection(fullInstallments.FourthInstallment.FourthInstallment, fourthInstallmentModel, 4, 3);
                OffersFourInstallmentsCollection.Add(fourthInstallmentModel);
            }
        }
        private void UpdateMainList(int installmentNumber)
        {
            switch (installmentNumber)
            {
                case 1:
                    OffersCollection = new ObservableCollection<CivilInsuranceSearchModel>(OffersOneInstallmentCollection);
                    break;
                case 2:
                    OffersCollection = new ObservableCollection<CivilInsuranceSearchModel>(OffersTwoInstallmentsCollection);
                    break;
                case 4:
                    OffersCollection = new ObservableCollection<CivilInsuranceSearchModel>(OffersFourInstallmentsCollection);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(installmentNumber), "Invalid installment number.");
            }
        }
        private void AddInstallments(CivilInsuranceSearchModel current, CivilInsuranceInstallments installments)
        {
            AddInstallmentToCollection(installments.FirstInstallment, current, 1, 0);
            AddInstallmentToCollection(installments.SecondInstallment, current, 2, 1);
            AddInstallmentToCollection(installments.ThirdInstallment, current, 3, 2);
            AddInstallmentToCollection(installments.FourthInstallment, current, 4, 3);
        }
        private void AddInstallmentToCollection(CivilInsuranceInstallment installment, CivilInsuranceSearchModel current, int number, int index)
        {
            if (installment != null && installment.PremiumWithTax != 0)
            {
                var currentInstallment = new InstallmentModel
                {
                    Number = number,
                    Index = index,
                    ShortText = Helpers.GetInsuranceNumberShortText(number),
                    Date = installment.PaymentDate,
                    Total = installment.PremiumWithTax,
                    PriceFormatted = Helpers.FormatPrice(installment.PremiumWithTax) + " лв.",
                    DateFormatted = installment.PaymentDate.HasValue ? installment.PaymentDate.Value.ToString("dd.MM.yyyy") : string.Empty,
                    Text = Helpers.GetInsuranceNumberText(number)
                };
                current.Installments.Add(currentInstallment);
            }
        }
        #endregion
    }
}
