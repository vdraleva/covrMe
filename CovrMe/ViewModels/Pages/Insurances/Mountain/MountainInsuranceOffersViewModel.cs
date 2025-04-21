using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Request.MountainInsurance;
using CovrMe.Models.Insurances.Request.TravelInsurance;
using CovrMe.Models.Insurances.Result.MountainInsurance;
using CovrMe.Models.Insurances.Result.TravelInsurance;
using CovrMe.Services.Contracts;
using CovrMe.Services.Implementation;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.Mountain;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.Mountain
{
    public partial class MountainInsuranceOffersViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private bool firstBorderSelected;
        private bool secondBorderSelected;
        private bool thirdBorderSelected;
        private bool forthBorderSelected;
        private int _insuranceSum = 5000;

        private string _groupUnder18 = "under18";
        private string _groupOver18 = "over18";

        private MountainInsuranceSearchModel _selectedOffer;

        private IInsuranceService _insuranceService;
        private ObservableCollection<MountainInsuranceSearchModel> _offersCollection;

        private MountainInsuranceOfferModel _mointainInfo;
        private List<InsuredUsersAgeTemplateModel> _usersAge;
        #endregion

        public MountainInsuranceOffersViewModel(IInsuranceService insuranceService)
        {
            this._insuranceService = insuranceService;
            ThirdBorderSelected = true;
            this.OffersCollection = new ObservableCollection<MountainInsuranceSearchModel>();
        }

        #region Collections

        public ObservableCollection<MountainInsuranceSearchModel> OffersCollection
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

        public MountainInsuranceSearchModel SelectedOffer
        {
            get { return _selectedOffer; }
            set { SetProperty(ref _selectedOffer, value); }
        }

        #endregion

        #region Commands
        [RelayCommand]
        private async void Continue(MountainInsuranceSearchModel selectedOffer)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                if(selectedOffer != null)
                {
                    this.SelectedOffer = selectedOffer;
                }

                this._mointainInfo.InsuranceSum = this._insuranceSum;

                var offer = new InsuranceOfferModel
                {
                    CompanyName = this.SelectedOffer.InsuranceCompanyName,
                    Price = this.SelectedOffer.Premium,                    
                    TotalPrice = this.SelectedOffer.Premium,
                    Tax = this.SelectedOffer.Tax,
                    PriceWithoutTaxFormatted = this.SelectedOffer.PremiumWithoutTaxFormatted,
                    TaxFormatted = this.SelectedOffer.TaxFormatted,
                    PriceFormatted = this.SelectedOffer.PremiumFormatted,
                    InsuranceType = (byte)InsuranceTypeEnum.Mountain,
                    CompanyLogo = this.SelectedOffer.LogoSrc,
                    StartDate = App.CalendarStartDate,
                    EndDate = App.CalendarEndDate,
                    StartDateFormatted = App.CalendarStartDate.ToString("dd.MM.yyyy"),
                    EndDateFormatted = App.CalendarEndDate.ToString("dd.MM.yyyy"),
                    Installment = 1,
                    MountainInsuranceInfo = this._mointainInfo
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"userAgeList", this._usersAge},
                        {"selectedOffer", offer},
                    };

                await Navigation.PushAsync<MountainInsuranceInfoPage>(parameters);
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
                        this._insuranceSum = 2000;

                        await GetOffers();

                        break;

                    case "Border2":
                        this.SecondBorderSelected = true;

                        this.FirstBorderSelected = false;
                        this.ThirdBorderSelected = false;
                        this.ForthBorderSelected = false;
                        this._insuranceSum = 3000;

                        await GetOffers();
                        break;

                    case "Border3":
                        this.ThirdBorderSelected = true;

                        this.FirstBorderSelected = false;
                        this.SecondBorderSelected = false;
                        this.ForthBorderSelected = false;
                        this._insuranceSum = 5000;
                        await GetOffers();

                        break;

                    case "Border4":
                        this.ForthBorderSelected = true;

                        this.FirstBorderSelected = false;
                        this.SecondBorderSelected = false;
                        this.ThirdBorderSelected = false;
                        this._insuranceSum = 10000;

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

            this._mointainInfo = query.FirstOrDefault(x => x.Key == "mountainInfo").Value as MountainInsuranceOfferModel;
            this._usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;

            Task.Run(async () => { await GetOffers(); }).Wait();
        }

        private async Task GetOffers()
        {
            var offersCollection = new ObservableCollection<MountainInsuranceSearchModel>();

            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var req = new MountainCalculationInput
            {
                StartDate = App.CalendarStartDate,
                EndDate = App.CalendarEndDate,
                NumberUnder18 = this.AgeGroupCount(this._groupUnder18),
                NumberOver18 = this.AgeGroupCount(this._groupOver18),
                InsuranceSum = this._insuranceSum,
                IsExtreme = this._mointainInfo.IsExtreme,
            };

            var offersResponse = await this._insuranceService.MountainCalculation(req, jwt, httpClient);

            foreach (var offer in offersResponse)
            {
                if (!string.IsNullOrEmpty(offer.CompanyName))
                {
                    var current = new MountainInsuranceSearchModel();
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

        private byte AgeGroupCount(string group)
        {
            byte count = 0;
            switch (group)
            {
                case "under18":
                    count = (byte)this._usersAge.Count(x => int.Parse(x.Age) < 18); break;

                case "over18":
                    count = (byte)this._usersAge.Count(x => int.Parse(x.Age) >= 18); break;
            }

            return count;
        }

        #endregion

    }
}
