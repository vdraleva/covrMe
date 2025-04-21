using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Insurances.Mountain;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.Mountain
{
    public partial class MountainInsuranceInfoPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private bool _firstCardOpened;
        private bool _secondCardOpened;
        private bool _thirdCardOpened;
        private bool _forthCardOpened;

        private string _firstCardArrow = GlobalConstants.PlusImg;
        private string _secondCardArrow = GlobalConstants.PlusImg;
        private string _thirdCardArrow = GlobalConstants.PlusImg;
        private string _forthCardArrow = GlobalConstants.PlusImg;

        private decimal _limit;
        private string _limitFormatted;

        private List<InsuredUsersAgeTemplateModel> _usersAge;
        private InsuranceOfferModel _selectedOffer;
        #endregion

        public MountainInsuranceInfoPageViewModel()
        {
            
        }

        #region Props
        public bool FirstCardOpened
        {
            get { return this._firstCardOpened; }
            set { SetProperty(ref this._firstCardOpened, value); }
        }
        public bool SecondCardOpened
        {
            get { return this._secondCardOpened; }
            set { SetProperty(ref this._secondCardOpened, value); }
        }
        public bool ThirdCardOpened
        {
            get { return this._thirdCardOpened; }
            set { SetProperty(ref this._thirdCardOpened, value); }
        }
        public bool ForthCardOpened
        {
            get { return this._forthCardOpened; }
            set { SetProperty(ref this._forthCardOpened, value); }
        }
        public string FirstCardArrow
        {
            get { return this._firstCardArrow; }
            set { SetProperty(ref this._firstCardArrow, value); }
        }
        public string SecondCardArrow
        {
            get { return this._secondCardArrow; }
            set { SetProperty(ref this._secondCardArrow, value); }
        }
        public string ThirdCardArrow
        {
            get { return this._thirdCardArrow; }
            set { SetProperty(ref this._thirdCardArrow, value); }
        }
        public string ForthCardArrow
        {
            get { return this._forthCardArrow; }
            set { SetProperty(ref this._forthCardArrow, value); }
        }

        public string LimitFormatted
        {
            get { return this._limitFormatted; }
            set { SetProperty(ref this._limitFormatted, value); }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async void Continue()
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
                        {"userAgeList", this._usersAge},
                        {"selectedOffer", this._selectedOffer},
                    };


                await Navigation.PushAsync<InsuranceOwnerPage>(parameters);
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
        void CardToggle(string toggleName)
        {
            switch (toggleName)
            {
                case "first":
                    this.FirstCardOpened = !this.FirstCardOpened;
                    this.FirstCardArrow = this.FirstCardOpened ? GlobalConstants.MinusImg : GlobalConstants.PlusImg;

                    // Close other cards
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.PlusImg;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.PlusImg;
                    this.ForthCardOpened = false;
                    this.ForthCardArrow = GlobalConstants.PlusImg;
                    break;

                case "second":
                    this.SecondCardOpened = !this.SecondCardOpened;
                    this.SecondCardArrow = this.SecondCardOpened ? GlobalConstants.MinusImg : GlobalConstants.PlusImg;

                    // Close other cards
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.PlusImg;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.PlusImg;
                    this.ForthCardOpened = false;
                    this.ForthCardArrow = GlobalConstants.PlusImg;
                    break;

                case "third":
                    this.ThirdCardOpened = !this.ThirdCardOpened;
                    this.ThirdCardArrow = this.ThirdCardOpened ? GlobalConstants.MinusImg : GlobalConstants.PlusImg;

                    // Close other cards
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.PlusImg;
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.PlusImg;
                    this.ForthCardOpened = false;
                    this.ForthCardArrow = GlobalConstants.PlusImg;
                    break;

                case "forth":
                    this.ForthCardOpened = !this.ForthCardOpened;
                    this.ForthCardArrow = this.ForthCardOpened ? GlobalConstants.MinusImg : GlobalConstants.PlusImg;

                    // Close other cards
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.PlusImg;
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.PlusImg;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.PlusImg;
                    break;

                default:
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.PlusImg;
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.PlusImg;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.PlusImg;
                    this.ForthCardOpened = false;
                    this.ForthCardArrow = GlobalConstants.PlusImg;
                    break;
            }
        }

        [RelayCommand]
        private async void GoToTermsTouristInsurance(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.ObshtiUsloviaMountainURL;
                await OpenBrowser(uri);
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
        private async void GoToInfoDocument(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.InfoDocumentMountainURL;
                await OpenBrowser(uri);
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
        private async void GoToPreDocumentInfo(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.PreddogovorenInfoMountainURL;
                await OpenBrowser(uri);
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
        private async void GoToPrivacyPolicyInfo(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.PrivacyPolicyMountainURL;
                await OpenBrowser(uri);
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
        private async void GoToPrivacyPolicy(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.PrivacyPolicyURL;
                await OpenBrowser(uri);
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
        private async void GoToContract(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.ContractURL;
                await OpenBrowser(uri);
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
        private async void GoToBankCardTerms(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.BankCardURL;
                await OpenBrowser(uri);
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
        private async Task OpenBrowser(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.FromHex("#50E3C6"),
                    PreferredControlColor = Color.FromHex("#C0C0C0"),
                });
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;
            this._usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            this.LimitFormatted = Helpers.FormatNumbers(this._selectedOffer.MountainInsuranceInfo.InsuranceSum) + " лв.";
        }

        #endregion
    }
}
