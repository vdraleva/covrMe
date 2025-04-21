using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Request.MyThings;
using CovrMe.Models.Insurances.Result.MyThings;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.MyThings;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.MyThings
{
    public partial class MyThingsInsuranceOffersViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private MyThingsInsuranceSearchModel _selectedOffer;
        private MyThingsInsuranceOfferModel _myThingsInfo;
        private IInsuranceService _insuranceService;
        private ObservableCollection<MyThingsInsuranceSearchModel> _offersCollection;

        #endregion

        public MyThingsInsuranceOffersViewModel(IInsuranceService insuranceService)
        {
            this._insuranceService = insuranceService;
            this.OffersCollection = new ObservableCollection<MyThingsInsuranceSearchModel>();
        }
        #region Collections

        public ObservableCollection<MyThingsInsuranceSearchModel> OffersCollection
        {
            get => _offersCollection;
            set => SetProperty(ref _offersCollection, value);
        }

        #endregion

        #region Props
        public MyThingsInsuranceSearchModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async void Continue(MyThingsInsuranceSearchModel selectedOffer)
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
                    TotalPrice = this.SelectedOffer.Premium,
                    PriceFormatted = this.SelectedOffer.PremiumFormatted,
                    Tax = this.SelectedOffer.Tax,
                    PriceWithoutTaxFormatted = this.SelectedOffer.PremiumWithoutTaxFormatted,
                    TaxFormatted = this.SelectedOffer.TaxFormatted,
                    InsuranceType = (byte)InsuranceTypeEnum.MyThings,
                    CompanyLogo = this.SelectedOffer.LogoSrc,
                    StartDate = App.CalendarStartDate,
                    EndDate = App.CalendarEndDate,
                    StartDateFormatted = App.CalendarStartDate.ToString("dd.MM.yyyy"),
                    EndDateFormatted = App.CalendarEndDate.ToString("dd.MM.yyyy"),
                    Installment = 1,
                    MyThingsInsuranceInfo = this._myThingsInfo
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"selectedOffer", offer},
                    };

                await Navigation.PushAsync<MyThingsInsuranceInfoPage>(parameters);
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

        private async Task GetOffers()
        {
            var offersCollection = new ObservableCollection<MyThingsInsuranceSearchModel>();

            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var req = new MyThingsCalculationInput
            {
                StartDate = App.CalendarStartDate,
                EndDate = App.CalendarEndDate,
                PropertyTypeId = this._myThingsInfo.PropertyTypeId,
                ObjectTypeId = this._myThingsInfo.ObjectTypeId,
                InsuranceSum = this._myThingsInfo.InsuranceSum,
                Questionnaire = this._myThingsInfo.Questionnaire
            };

            var offersResponse = await this._insuranceService.MyThingsCalculation(req, jwt, httpClient);

            foreach (var offer in offersResponse)
            {
                if (!string.IsNullOrEmpty(offer.CompanyName))
                {
                    var current = new MyThingsInsuranceSearchModel();
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._myThingsInfo = query.FirstOrDefault(x => x.Key == "myThingsInfo").Value as MyThingsInsuranceOfferModel;

            Task.Run(async () => { await GetOffers(); }).Wait();
        }

        #endregion
    }
}
