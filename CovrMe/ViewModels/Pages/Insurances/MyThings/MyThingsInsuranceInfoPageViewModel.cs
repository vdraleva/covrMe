using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Insurances.MyThings;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.MyThings
{
    public partial class MyThingsInsuranceInfoPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private bool firstCardOpened;
        private bool secondCardOpened;
        private bool thirdCardOpened;
        private bool forthCardOpened;

        private string firstCardArrow = GlobalConstants.PlusImg;
        private string secondCardArrow = GlobalConstants.PlusImg;
        private string thirdCardArrow = GlobalConstants.PlusImg;
        private string forthCardArrow = GlobalConstants.PlusImg;

        private decimal _insuranceSum;
        private string _insuranceSumFormatted;

        private InsuranceOfferModel _selectedOffer;

        public MyThingsInsuranceInfoPageViewModel()
        {
            
        }
        #endregion

        #region Props
        public bool FirstCardOpened
        {
            get { return this.firstCardOpened; }
            set { SetProperty(ref this.firstCardOpened, value); }
        }
        public bool SecondCardOpened
        {
            get { return this.secondCardOpened; }
            set { SetProperty(ref this.secondCardOpened, value); }
        }
        public bool ThirdCardOpened
        {
            get { return this.thirdCardOpened; }
            set { SetProperty(ref this.thirdCardOpened, value); }
        }
        public bool ForthCardOpened
        {
            get { return this.forthCardOpened; }
            set { SetProperty(ref this.forthCardOpened, value); }
        }
        public string FirstCardArrow
        {
            get { return this.firstCardArrow; }
            set { SetProperty(ref this.firstCardArrow, value); }
        }
        public string SecondCardArrow
        {
            get { return this.secondCardArrow; }
            set { SetProperty(ref this.secondCardArrow, value); }
        }
        public string ThirdCardArrow
        {
            get { return this.thirdCardArrow; }
            set { SetProperty(ref this.thirdCardArrow, value); }
        }
        public string ForthCardArrow
        {
            get { return this.forthCardArrow; }
            set { SetProperty(ref this.forthCardArrow, value); }
        }

        public decimal InsuranceSum
        {
            get { return this._insuranceSum; }
            set { SetProperty(ref this._insuranceSum, value); }
        }

        public string InsuranceSumFormatted
        {
            get { return this._insuranceSumFormatted; }
            set { SetProperty(ref this._insuranceSumFormatted, value); }
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
        private async void GoToTermsMyThingsInsurance(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();

                string url = string.Empty;

                if(this._selectedOffer.MyThingsInsuranceInfo.PropertyTypeId == (int)MyThingsPropertyTypeEnum.Glasses)
                {
                    url = GlobalConstants.ObshtiUsloviaGlassesURL;
                }
                else
                {
                    url = GlobalConstants.ObshtiUsloviaBycicleURL;
                }
                
                await OpenBrowser(url);
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

                string url = string.Empty;

                if (this._selectedOffer.MyThingsInsuranceInfo.PropertyTypeId == (int)MyThingsPropertyTypeEnum.Glasses)
                {
                    url = GlobalConstants.InfoDocumentGlassesURL;
                }
                else
                {
                    url = GlobalConstants.InfoDocumentBycicleURL;
                }

                await OpenBrowser(url);
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

                string url = string.Empty;

                if (this._selectedOffer.MyThingsInsuranceInfo.PropertyTypeId == (int)MyThingsPropertyTypeEnum.Glasses)
                {
                    url = GlobalConstants.PreddogovorenInfoGlassesURL;
                }
                else
                {
                    url = GlobalConstants.PreddogovorenInfoBycicleURL;
                }

                await OpenBrowser(url);
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
                string url = string.Empty;

                if (this._selectedOffer.MyThingsInsuranceInfo.PropertyTypeId == (int)MyThingsPropertyTypeEnum.Glasses)
                {
                    url = GlobalConstants.PrivacyPolicyGlassesURL;
                }
                else
                {
                    url = GlobalConstants.PrivacyPolicyBycicleURL;
                }

                await OpenBrowser(url);
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
        private async void GoToSpecialConditions(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string url = string.Empty;

                if (this._selectedOffer.MyThingsInsuranceInfo.PropertyTypeId == (int)MyThingsPropertyTypeEnum.Glasses)
                {
                    url = GlobalConstants.SpecialConditionsGlassesURL;
                }
                else
                {
                    url = GlobalConstants.SpecialConditionsBycicleURL;
                }

                await OpenBrowser(url);
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

            this.InsuranceSumFormatted = Helpers.FormatNumbers(this._selectedOffer.MyThingsInsuranceInfo.InsuranceSum) + " лв.";
        }

        #endregion`
    }
}
