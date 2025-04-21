using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Insurances.CivilInsurance.Request;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using System.Collections.ObjectModel;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using CovrMe.Shared;
using CovrMe.Models.Vehicles;
using CovrMe.Models.Users;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Result.CivilInsurances;
using CovrMe.Models.Vehicles.Result;

namespace CovrMe.ViewModels.Pages.Insurances.CivilInsurance
{
    public partial class CivilInsuranceOffersViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private int _installment = 1;
        private int _oneInstallment = 1;
        private int _twoInstallments = 2;
        private int _fourInstallments = 4;

        private InsuranceVehicleInfo _vehicleInfo;
        private InsuranceUserInfoModel _ownerUserInfo;
        private InsuranceUserInfoModel _usualUserInfo;

        private string _vehicle;

        private RegCertificateResultModel _regCertificateModel;

        //collections
        private ObservableCollection<CivilInsuranceSearchModel> _offersCollection;
        private ObservableCollection<CivilInsuranceSearchModel> _offersOneInstallmentCollection;

        //insurance
        private bool _isOnePayment;
        private bool _isTwoPayment;
        private bool _isFourPayment;
        private string _onePayBtnColor;
        private string _twoPayBtnColor;
        private string _fourPayBtnColor;
        private CivilInsuranceSearchModel _selectedOffer;
        private List<string> _availableOffersCompanyNames;


        //services
        private IInsuranceService _insuranceService;

        #endregion

        public CivilInsuranceOffersViewModel(IInsuranceService insuranceService)
        {
            IsOnePaymentsVisible = true;
            IsTwoPaymentsVisible = false;
            IsFourPaymentsVisible = false;

            this._insuranceService = insuranceService;

            this.OffersCollection = new ObservableCollection<CivilInsuranceSearchModel>();
            this.OffersOneInstallmentCollection = new ObservableCollection<CivilInsuranceSearchModel>();

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
                    Installments = this.SelectedOffer.Installments
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

        [RelayCommand]
        private async void MoreOffers()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var parameters = new Dictionary<string, object>
                    {
                        {"usualUserInfo", _usualUserInfo},
                        {"ownerUserInfo", _ownerUserInfo},
                        {"userVehicleInfo", _vehicleInfo},
                        {"availableOffersCompanyNames", _availableOffersCompanyNames},
                        {"oneInstallmentOffersCollection", _offersOneInstallmentCollection},
                        {"ocrRegCertificateModel", _regCertificateModel},
                    };

                await Navigation.PushAsync<CivilInsuranceVehicleAdditionalInfoPage>(parameters);
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

                        await GetOffers(_oneInstallment);

                        break;

                    case "two":
                        IsTwoPaymentsVisible = true;
                        IsOnePaymentsVisible = false;
                        IsFourPaymentsVisible = false;

                        TwoPayBtnColor = GlobalConstants.ActiveColorPurple;
                        OnePayBtnColor = GlobalConstants.WhiteColor;
                        FourPayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 2;

                        await GetOffers(_twoInstallments);
                        break;

                    case "four":
                        IsFourPaymentsVisible = true;
                        IsOnePaymentsVisible = false;
                        IsTwoPaymentsVisible = false;

                        FourPayBtnColor = GlobalConstants.ActiveColorPurple;
                        TwoPayBtnColor = GlobalConstants.WhiteColor;
                        OnePayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 4;

                        await GetOffers(_fourInstallments);
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
            this._regCertificateModel = query.FirstOrDefault(x => x.Key == "ocrRegCertificateModel").Value as RegCertificateResultModel;

            this.Vehicle = this._vehicleInfo.VehicleBrand + " " + this._vehicleInfo.VehicleModel;

            Task.Run(async () => { await GetOffers(_oneInstallment); }).Wait();
        }

        private async Task GetOffers(int installment)
        {
            var offersCollection = new ObservableCollection<CivilInsuranceSearchModel>();

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

            this._availableOffersCompanyNames = new List<string>();

            foreach (var offer in offersResponse.InsuranceOffers)
            {
                if (offer.Success == (int)GeneralStatusEnum.Success)
                {
                    var current = new CivilInsuranceSearchModel();

                    if (offer.CivilInsuranceOffer != null)
                    {
                        if (offer.CivilInsuranceOffer.CivilInsuranceCalculation != null)
                        {
                            if (offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments != null)
                            {
                                current.InsuranceCompanyName = offer.InsuranceCompanyName;
                                current.Premium = offer.CivilInsuranceOffer.CivilInsuranceCalculation.PremiumWithTax;
                                current.PremiumFormatted = current.Premium.ToString() + " лв.";

                                if (offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.FirstInstallment != null &&
                                    offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.FirstInstallment.PremiumWithTax != 0)
                                {
                                    var currentInstallment = new InstallmentModel();
                                    currentInstallment.Number = 1;
                                    currentInstallment.Index = 0;
                                    currentInstallment.ShortText = Helpers.GetInsuranceNumberShortText(currentInstallment.Number);
                                    currentInstallment.Date = offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.FirstInstallment.PaymentDate;
                                    currentInstallment.Total = offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.FirstInstallment.PremiumWithTax;
                                    currentInstallment.PriceFormatted = Helpers.FormatPrice(currentInstallment.Total) + " лв.";
                                    currentInstallment.DateFormatted = currentInstallment.Date.HasValue ? currentInstallment.Date.Value.ToString("dd.MM.yyyy") : string.Empty;
                                    currentInstallment.Text = Helpers.GetInsuranceNumberText(currentInstallment.Number);

                                    current.Installments.Add(currentInstallment);
                                }

                                if (offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.SecondInstallment != null &&
                                    offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.SecondInstallment.PremiumWithTax != 0)
                                {
                                    var currentInstallment = new InstallmentModel();
                                    currentInstallment.Number = 2;
                                    currentInstallment.Index = 1;
                                    currentInstallment.ShortText = Helpers.GetInsuranceNumberShortText(currentInstallment.Number);
                                    currentInstallment.Date = offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.SecondInstallment.PaymentDate;
                                    currentInstallment.Total = offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.SecondInstallment.PremiumWithTax;
                                    currentInstallment.PriceFormatted = currentInstallment.Total.ToString() + " лв.";
                                    currentInstallment.DateFormatted = currentInstallment.Date.HasValue ? currentInstallment.Date.Value.ToString("dd.MM.yyyy") : string.Empty;
                                    currentInstallment.Text = Helpers.GetInsuranceNumberText(currentInstallment.Number);

                                    current.Installments.Add(currentInstallment);
                                }

                                if (offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.ThirdInstallment != null &&
                                    offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.ThirdInstallment.PremiumWithTax != 0)
                                {
                                    var currentInstallment = new InstallmentModel();
                                    currentInstallment.Number = 3;
                                    currentInstallment.Index = 2;
                                    currentInstallment.ShortText = Helpers.GetInsuranceNumberShortText(currentInstallment.Number);
                                    currentInstallment.Date = offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.ThirdInstallment.PaymentDate;
                                    currentInstallment.Total = offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.ThirdInstallment.PremiumWithTax;
                                    currentInstallment.PriceFormatted = currentInstallment.Total.ToString() + " лв.";
                                    currentInstallment.DateFormatted = currentInstallment.Date.HasValue ? currentInstallment.Date.Value.ToString("dd.MM.yyyy") : string.Empty;
                                    currentInstallment.Text = Helpers.GetInsuranceNumberText(currentInstallment.Number);

                                    current.Installments.Add(currentInstallment);
                                }

                                if (offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.FourthInstallment != null &&
                                    offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.FourthInstallment.PremiumWithTax != 0)
                                {
                                    var currentInstallment = new InstallmentModel();
                                    currentInstallment.Number = 4;
                                    currentInstallment.Index = 3;
                                    currentInstallment.ShortText = Helpers.GetInsuranceNumberShortText(currentInstallment.Number);
                                    currentInstallment.Date = offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.FourthInstallment.PaymentDate;
                                    currentInstallment.Total = offer.CivilInsuranceOffer.CivilInsuranceCalculation.Installments.FourthInstallment.PremiumWithTax;
                                    currentInstallment.PriceFormatted = currentInstallment.Total.ToString() + " лв.";
                                    currentInstallment.DateFormatted = currentInstallment.Date.HasValue ? currentInstallment.Date.Value.ToString("dd.MM.yyyy") : string.Empty;
                                    currentInstallment.Text = Helpers.GetInsuranceNumberText(currentInstallment.Number);

                                    current.Installments.Add(currentInstallment);
                                }

                                this._availableOffersCompanyNames.Add(current.InsuranceCompanyName.ToLower());

                                current.LogoSrc = Helpers.GetImageSrc(current.InsuranceCompanyName);
                                offersCollection.Add(current);
                            }
                        }
                    }
                }
            }

            if (installment == 1)
            {
                this.OffersOneInstallmentCollection.Clear();
                this.OffersOneInstallmentCollection = offersCollection;
            }

            this.OffersCollection.Clear();
            this.OffersCollection = offersCollection;
        }

        #endregion
    }
}
