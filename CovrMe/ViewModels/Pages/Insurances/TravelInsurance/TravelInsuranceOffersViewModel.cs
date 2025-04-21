using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Request.TravelInsurance;
using CovrMe.Models.Insurances.Result.TravelInsurance;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using Maui.Plugins.PageResolver;
using System.Collections.ObjectModel;

namespace CovrMe.ViewModels.Pages.Insurances.TravelInsurance
{
    public partial class TravelInsuranceOffersViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private bool firstBorderSelected;
        private bool secondBorderSelected;
        private bool thirdBorderSelected;
        private bool forthBorderSelected;
        private decimal _limit = 15000;

        private string _groupUnder18 = "under18";
        private string _group18To65 = "18to65";
        private string _group66To70 = "66to70";
        private string _group71To75 = "71to75";
        private string _group76To80 = "76to80";

        private TravelInsuranceSearchModel _selectedOffer;

        private IInsuranceService _insuranceService;
        private ObservableCollection<TravelInsuranceSearchModel> _offersCollection;

        private TravelInsuranceOfferModel _travelInfo;
        private List<InsuredUsersAgeTemplateModel> _usersAge;
        #endregion
        public TravelInsuranceOffersViewModel(IInsuranceService insuranceService)
        {
            this._insuranceService = insuranceService;
            SecondBorderSelected = true;
            this.OffersCollection = new ObservableCollection<TravelInsuranceSearchModel>();
        }

        #region Collections

        public ObservableCollection<TravelInsuranceSearchModel> OffersCollection
        {
            get => _offersCollection;
            set => SetProperty(ref _offersCollection, value);
        }

        #endregion

        #region Props
        public bool FirstBorderSelected
        {
            get { return this.firstBorderSelected; }
            set { SetProperty(ref this.firstBorderSelected, value); }
        }
        public bool SecondBorderSelected
        {
            get { return this.secondBorderSelected; }
            set { SetProperty(ref this.secondBorderSelected, value); }
        }
        public bool ThirdBorderSelected
        {
            get { return this.thirdBorderSelected; }
            set { SetProperty(ref this.thirdBorderSelected, value); }
        }
        public bool ForthBorderSelected
        {
            get { return this.forthBorderSelected; }
            set { SetProperty(ref this.forthBorderSelected, value); }
        }

        public TravelInsuranceSearchModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }

        #endregion


        #region Commands
        [RelayCommand]
        private async void Continue(TravelInsuranceSearchModel selectedOffer)
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

                this._travelInfo.Limit = this._limit;

                var offer = new InsuranceOfferModel
                {
                    CompanyName = this.SelectedOffer.InsuranceCompanyName,
                    Price = this.SelectedOffer.Premium,
                    TotalPrice = this.SelectedOffer.Premium,
                    Tax = this.SelectedOffer.Tax,
                    PriceWithoutTaxFormatted = this.SelectedOffer.PremiumWithoutTaxFormatted,
                    TaxFormatted = this.SelectedOffer.TaxFormatted,
                    PriceFormatted = this.SelectedOffer.PremiumFormatted,
                    InsuranceType = (byte)InsuranceTypeEnum.Travel,
                    CompanyLogo = this.SelectedOffer.LogoSrc,
                    StartDate = App.CalendarStartDate,
                    EndDate = App.CalendarEndDate,
                    StartDateFormatted = App.CalendarStartDate.ToString("dd.MM.yyyy"),
                    EndDateFormatted = App.CalendarEndDate.ToString("dd.MM.yyyy"),
                    Installment = 1,
                    TravelInsuranceInfo = this._travelInfo                   
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"userAgeList", this._usersAge},
                        {"selectedOffer", offer},
                    };

                await Navigation.PushAsync<TravelInsuranceInfoPage>(parameters);
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
        public async Task OnBorderTapped(string borderNumber)
        {
            try
            {

                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;
                ShowLoading();

                switch (borderNumber)
                {
                    case "Border1":
                        this.FirstBorderSelected = true;

                        this.SecondBorderSelected = false;
                        this.ThirdBorderSelected = false;
                        this.ForthBorderSelected = false;
                        this._limit = 10000;

                        await GetOffers();

                        break;

                    case "Border2":
                        this.SecondBorderSelected = true;

                        this.FirstBorderSelected = false;
                        this.ThirdBorderSelected = false;
                        this.ForthBorderSelected = false;
                        this._limit = 15000;

                        await GetOffers();
                        break;

                    case "Border3":
                        this.ThirdBorderSelected = true;

                        this.FirstBorderSelected = false;
                        this.SecondBorderSelected = false;
                        this.ForthBorderSelected = false;
                        this._limit = 30000;
                        await GetOffers();

                        break;

                    case "Border4":
                        this.ForthBorderSelected = true;

                        this.FirstBorderSelected = false;
                        this.SecondBorderSelected = false;
                        this.ThirdBorderSelected = false;
                        this._limit = 50000;

                        await GetOffers();

                        break;

                    default:
                        this.FirstBorderSelected = true;

                        this.SecondBorderSelected = false;
                        this.ThirdBorderSelected = false;
                        this.ForthBorderSelected = false;
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
        #endregion

        #region Methods
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._travelInfo = query.FirstOrDefault(x => x.Key == "travelInfo").Value as TravelInsuranceOfferModel;
            this._usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;

            Task.Run(async () => { await GetOffers(); }).Wait();
        }

        private async Task GetOffers()
        {
            var offersCollection = new ObservableCollection<TravelInsuranceSearchModel>();

            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var req = new TravelCalculationInput
            {
                PolicyType = this._travelInfo.PolicyType,
                StartDate = App.CalendarStartDate,
                EndDate = App.CalendarEndDate,
                TripPurpose = this._travelInfo.TripPurpose,
                Territory = this._travelInfo.Territory,
                Limit = this._limit,
                NumberUnder18 = this.AgeGroupCount(this._groupUnder18),
                Number18To65 = this.AgeGroupCount(this._group18To65),
                Number66To70 = this.AgeGroupCount(this._group66To70),
                Number71To75 = this.AgeGroupCount(this._group71To75),
                Number76To80 = this.AgeGroupCount(this._group76To80),
            };

            var offersResponse = await this._insuranceService.TravelCalculation(req, jwt, httpClient);

            foreach (var offer in offersResponse)
            {
                if (!string.IsNullOrEmpty(offer.CompanyName))
                {
                    var current = new TravelInsuranceSearchModel();
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
        #endregion
        private byte AgeGroupCount(string group)
        {
            byte count = 0;
            switch (group)
            {
                case "under18":
                    count =  (byte)this._usersAge.Count(x => int.Parse(x.Age) < 18); break;

                case "18to65":
                    count = (byte)this._usersAge.Count(x => int.Parse(x.Age) >= 18 && int.Parse(x.Age) <= 65); break;

                case "66to70":
                    count = (byte)this._usersAge.Count(x => int.Parse(x.Age) >= 66 && int.Parse(x.Age) <= 70); break;

                case "71to75":
                    count = (byte)this._usersAge.Count(x => int.Parse(x.Age) >= 71 && int.Parse(x.Age) <= 75); break;

                case "76to80":
                    count = (byte)this._usersAge.Count(x => int.Parse(x.Age) >= 76 && int.Parse(x.Age) < 80); break;
            }

            return count;
        }
    }
}
