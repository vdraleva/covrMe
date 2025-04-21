using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Models.Users;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Payment;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.MyThings
{
    public partial class MyThingsInsuranceDocumentsPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields

        private InsuranceOfferModel _selectedOffer;
        private InsuranceUserInfoModel _userInfo;

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
                        {"ownerUserInfo", this._userInfo},
                    };

                await Navigation.PushAsync<PrePaymentPage>(parameters);
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
        private async void GoToTermsInsurance(object obj)
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

        [RelayCommand]
        private async void GoToPredgovorInfo(object obj)
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
        private async void GoToPrivacyPolicyMyThingsInfo(object obj)
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

        #endregion

        #region Methods

        private async Task OpenBrowser(string uri)
        {
            await Browser.OpenAsync(uri, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#50E3C6"),
                PreferredControlColor = Color.FromHex("#C0C0C0"),
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._selectedOffer = query.FirstOrDefault(x => x.Key == "selectedOffer").Value as InsuranceOfferModel;
            this._userInfo = query.FirstOrDefault(x => x.Key == "userInfo").Value as InsuranceUserInfoModel;
        }

        #endregion
    }
}
