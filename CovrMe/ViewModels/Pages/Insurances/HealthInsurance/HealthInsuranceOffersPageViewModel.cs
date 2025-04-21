using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Request.HealthInsurance;
using CovrMe.Models.Insurances.Result.HealthInsurance;
using CovrMe.Models.Insurances.Result.MountainInsurance;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.HealthInsurance;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.HealthInsurance
{
    public partial class HealthInsuranceOffersPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private int _installment = 1;
        private int _oneInstallment = 1;
        private int _twoInstallments = 2;
        private int _fourInstallments = 4;
        private int _twelveInstallments = 12;
        //insurance
        private bool _isOnePayment;
        private bool _isTwoPayment;
        private bool _isFourPayment;
        private bool _isTwelvePayment;
        private string _onePayBtnColor;
        private string _twoPayBtnColor;
        private string _fourPayBtnColor;
        private string _twelvePayBtnColor;

        private List<InsuredUsersAgeTemplateModel> _usersAge;
        private HealthInsuranceOfferModel _healthInfo;
        private IInsuranceService _insuranceService;
        private HealthInsuranceSearchModel _selectedOffer;

        //collections
        private ObservableCollection<HealthInsuranceSearchModel> _offersCollection;

        #endregion

        public HealthInsuranceOffersPageViewModel(IInsuranceService insuranceService)
        {
            this._insuranceService = insuranceService;
            this.OffersCollection = new ObservableCollection<HealthInsuranceSearchModel>();
            IsOnePaymentsVisible = true;
            IsTwoPaymentsVisible = false;
            IsFourPaymentsVisible = false;
            IsTwelvePaymentVisible = false;

            OnePayBtnColor = GlobalConstants.ActiveColorPurple;
            TwoPayBtnColor = GlobalConstants.WhiteColor;
            FourPayBtnColor = GlobalConstants.WhiteColor;
            TwelvePayBtnColor = GlobalConstants.WhiteColor;
        }

        #region Collections

        public ObservableCollection<HealthInsuranceSearchModel> OffersCollection
        {
            get => _offersCollection;
            set => SetProperty(ref _offersCollection, value);
        }

        #endregion

        #region Props
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

        public bool IsTwelvePaymentVisible
        {
            get { return _isTwelvePayment; }
            set { SetProperty(ref _isTwelvePayment, value); }
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
        public string TwelvePayBtnColor
        {
            get { return _twelvePayBtnColor; }
            set { SetProperty(ref _twelvePayBtnColor, value); }
        }

        public HealthInsuranceSearchModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async void Continue(HealthInsuranceSearchModel selectedOffer)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                if (selectedOffer != null)
                {
                    this.SelectedOffer = selectedOffer;
                }
               
                var offer = new InsuranceOfferModel
                {
                    CompanyName = this.SelectedOffer.InsuranceCompanyName,
                    Price = this.SelectedOffer.Premium,
                    PriceFormatted = this.SelectedOffer.PremiumFormatted,
                    TotalPrice = this.SelectedOffer.Premium,
                    Tax = this.SelectedOffer.Tax,
                    PriceWithoutTaxFormatted = this.SelectedOffer.PremiumWithoutTaxFormatted,
                    TaxFormatted = this.SelectedOffer.TaxFormatted,
                    InsuranceType = (byte)InsuranceTypeEnum.Health,
                    CompanyLogo = this.SelectedOffer.LogoSrc,
                    StartDate = App.CalendarStartDate,
                    EndDate = App.CalendarEndDate,
                    StartDateFormatted = App.CalendarStartDate.ToString("dd.MM.yyyy"),
                    EndDateFormatted = App.CalendarEndDate.ToString("dd.MM.yyyy"),
                    Installment = this._oneInstallment,
                    HealthInsuranceInfo = this._healthInfo
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"userAgeList", this._usersAge},
                        {"selectedOffer", offer},
                    };

                await Navigation.PushAsync<HealthDeclarationPage>(parameters);
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
                        IsTwelvePaymentVisible = false;

                        OnePayBtnColor = GlobalConstants.ActiveColorPurple;
                        TwoPayBtnColor = GlobalConstants.WhiteColor;
                        FourPayBtnColor = GlobalConstants.WhiteColor;
                        TwelvePayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 1;

                        await GetOffers(_oneInstallment);

                        break;

                    case "two":
                        IsTwoPaymentsVisible = true;
                        IsOnePaymentsVisible = false;
                        IsFourPaymentsVisible = false;
                        IsTwelvePaymentVisible = false;

                        TwoPayBtnColor = GlobalConstants.ActiveColorPurple;
                        OnePayBtnColor = GlobalConstants.WhiteColor;
                        FourPayBtnColor = GlobalConstants.WhiteColor;
                        TwelvePayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 2;

                        await GetOffers(_twoInstallments);
                        break;

                    case "four":
                        IsFourPaymentsVisible = true;
                        IsOnePaymentsVisible = false;
                        IsTwoPaymentsVisible = false;
                        IsTwelvePaymentVisible = false;

                        FourPayBtnColor = GlobalConstants.ActiveColorPurple;
                        TwoPayBtnColor = GlobalConstants.WhiteColor;
                        OnePayBtnColor = GlobalConstants.WhiteColor;
                        TwelvePayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 4;

                        await GetOffers(_fourInstallments);
                        break;

                    case "twelve":
                        IsTwelvePaymentVisible = true;
                        IsFourPaymentsVisible = false;
                        IsOnePaymentsVisible = false;
                        IsTwoPaymentsVisible = false;

                        TwelvePayBtnColor = GlobalConstants.ActiveColorPurple;
                        TwoPayBtnColor = GlobalConstants.WhiteColor;
                        OnePayBtnColor = GlobalConstants.WhiteColor;
                        FourPayBtnColor = GlobalConstants.WhiteColor;

                        Installment = 12;

                        await GetOffers(_twelveInstallments);
                        break;

                    default:
                        IsOnePaymentsVisible = true;
                        IsTwoPaymentsVisible = false;
                        IsFourPaymentsVisible = false;
                        IsTwelvePaymentVisible = false;

                        OnePayBtnColor = GlobalConstants.ActiveColorPurple;
                        TwoPayBtnColor = GlobalConstants.WhiteColor;
                        FourPayBtnColor = GlobalConstants.WhiteColor;
                        TwelvePayBtnColor = GlobalConstants.WhiteColor;
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


            #endregion
        }

        private async Task GetOffers(int installments)
        {
            var offersCollection = new ObservableCollection<HealthInsuranceSearchModel>();

            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var req = new HealthCalculationInput
            {
                StartDate = App.CalendarStartDate,
                EndDate = App.CalendarEndDate,
                PacketId = this._healthInfo.PacketId,
                InstallmentCount = installments,
                IsFamily = this._healthInfo.IsFamily,
                InsuredBirthDate = this.GetInsuredUsersBirthYears()
            };

            var offersResponse = await this._insuranceService.HealthCalculation(req, jwt, httpClient);

            foreach (var offer in offersResponse)
            {
                if (!string.IsNullOrEmpty(offer.CompanyName))
                {
                    var current = new HealthInsuranceSearchModel();
                    current.InsuranceCompanyName = offer.CompanyName;
                    current.Premium = offer.Premium;
                    current.PremiumFormatted = Helpers.FormatPrice(offer.Premium) + " лв.";
                    current.PremiumWithoutTax = offer.PremiumWithoutTax;
                    current.PremiumWithoutTaxFormatted = Helpers.FormatPrice(offer.PremiumWithoutTax) + " лв.";
                    current.Tax = offer.Tax;
                    current.TaxFormatted = Helpers.FormatPrice(offer.Tax) + " лв.";
                    current.LogoSrc = Helpers.GetImageSrc(current.InsuranceCompanyName);

                    offersCollection.Add(current);
                }
            }

            this.OffersCollection.Clear();
            this.OffersCollection = offersCollection;
            this.SelectedOffer = this.OffersCollection.FirstOrDefault();
        }

        public List<DateTime> GetInsuredUsersBirthYears()
        {
            var result = new List<DateTime>();

            foreach (var item in this._usersAge)
            {
                int negativeNumber = int.Parse(item.Age) * -1;
                var current = DateTime.Now.AddYears(negativeNumber);

                result.Add(current);
            }

            return result;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            _healthInfo = query.FirstOrDefault(x => x.Key == "healthInfo").Value as HealthInsuranceOfferModel;
            _usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;
            Task.Run(async () => { await GetOffers(_oneInstallment); }).Wait();
        }

        
    }
}
