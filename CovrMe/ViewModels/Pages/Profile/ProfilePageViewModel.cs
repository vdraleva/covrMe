using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models.Users.Request;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Profile;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Profile
{
    public partial class ProfilePageViewModel : BaseViewModel
    {
        private IUserService _userService;
        public ProfilePageViewModel(IUserService userService) 
        {
            _userService = userService;
        }

        #region Props
        #endregion

        #region Commands

        [RelayCommand]
        private async Task DeleteProfile()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                ShowLoading();

                var answer = await App.DisplayAlert(App.MESSAGE_HEADER_ATT, "Сигурни ли сте, че искате да изтриете профила си ?", App.MESSAGE_OK, App.MESSAGE_CANCEL);

                if (answer)
                {
                    var httpClient = HttpClientFactory.Create();
                    var userId = App.UserId;
                    var jwt = App.JwtToken;
                    var email = App.Email;

                    var req = new DeleteUserInput
                    {
                        UserId = userId,
                        Email = email
                    };

                    var result = await this._userService.DeleteUser(req, httpClient, jwt);

                    if(result.Code == (int)GeneralStatusEnum.Success)
                    {
                        await App.DisplayAlert(App.MESSAGE_Success, MessageConstants.DeleteUserSuccess, App.MESSAGE_OK);
                        App.JwtToken = string.Empty;
                        App.UserId = string.Empty;
                        App.Email = string.Empty;
                        App.UserName = string.Empty;
                        await Navigation.PushAsync<LoginPage>();
                    }
                    else
                    {
                        await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, MessageConstants.GeneralError, App.MESSAGE_OK);
                    }
                }               
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToPersonalDataPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                ShowLoading();

                await Navigation.PushAsync<PersonalDataPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToCarDataPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                ShowLoading();

                await Navigation.PushAsync<CarDataPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToFamilyFriendsDataPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                ShowLoading();

                await Navigation.PushAsync<FamilyFriendsDataPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToMyInsurancesPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                ShowLoading();

                await Navigation.PushAsync<MyInsurancesPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async void GoToInsuranceInfo(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.InsuranceContactForm;
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
        private async void GoToRegisterPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();
                await Navigation.PushAsync<RegisterPage>();
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
        private async void LogOut()
        {
            try
            {
                App.JwtToken = string.Empty;
                App.UserId = string.Empty;
                App.Email = string.Empty;
                App.UserName = string.Empty;

                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
        }
        #endregion

        #region Private methods
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
                ;
                // An unexpected error occured. No browser may be installed on the device.
            }
        }        
        #endregion
    }
}
