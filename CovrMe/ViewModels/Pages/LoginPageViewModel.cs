using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Models.Users.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.ViewModels.Popups;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;
using System.Text.Json;

namespace CovrMe.ViewModels.Pages
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        #region Fields
        private string email;
        private string password;
        private bool isValid = true;
        private bool isError = false;

        private bool passwordError;
        private bool emailError;
        private HttpClient _httpClient;

        private IAuthenticationService _authenticationService;
        private IPopupService _popupService;
        #endregion
        public LoginPageViewModel(IAuthenticationService authenticationService, IPopupService popupService)
        {
            this._authenticationService = authenticationService;
            this._popupService = popupService;
        }

        #region Props

        public bool EmailError
        {
            get { return emailError; }
            set
            {
                if (emailError != value)
                {
                    emailError = value;
                    OnPropertyChanged(nameof(EmailError));
                }
            }
        }
        public bool PasswordError
        {
            get { return passwordError; }
            set
            {
                if (passwordError != value)
                {
                    passwordError = value;
                    OnPropertyChanged(nameof(PasswordError));
                }
            }
        }
        public bool IsError
        {
            get { return isError; }
            set
            {
                if (isError != value)
                {
                    isError = value;
                    OnPropertyChanged(nameof(IsError));
                }
            }
        }
        public string Email
        {
            get { return this.email; }
            set { SetProperty(ref this.email, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetProperty(ref this.password, value); }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async Task Login()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                ShowLoading();

                var validateInput = this.ValidateInput();

                if (!validateInput.IsValid)
                {
                    throw new InvalidOperationException(MessageConstants.RequiredAll);
                }

                var email = this.Email.TrimEnd();
                var password = this.Password.TrimEnd();

                var httpClient = HttpClientFactory.Create();

                var loginResult = await this._authenticationService.Login(email, password, httpClient);

                if (loginResult.Code == (int)GeneralStatusEnum.Success)
                {
                    AttachUserData(loginResult.User.Id, loginResult.User.FirstName, loginResult.User.PhoneNumber, loginResult.User.Email, loginResult.Jwt);
                    await Navigation.PushAsync<HomePage>();
                }
                else
                {
                    if (loginResult.Message == MessageConstants.Unsuccess)
                    {
                        throw new Exception(MessageConstants.GeneralError);
                    }
                    else
                    {
                        throw new Exception(loginResult.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                HideLoading();
                IsBusy = false;
                if (isValid)
                {
                    this.Email = "";
                    this.Password = "";
                }
            }
        }

        [RelayCommand]
        private async Task OAuthLogin(string authProvider)
        {
            try
            {
                WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                 new Uri($"{this._authenticationService.GetBaseUrl()}auth/{authProvider.ToLower()}/login"),
                 new Uri("myapp://"));

                LoginResultModel result = JsonSerializer.Deserialize<LoginResultModel>(Uri.UnescapeDataString(authResult.Get("loginResult")));
                
                AttachUserData(result.User.Id, result.User.FirstName, result.User.PhoneNumber, result.User.Email, result.Jwt);

                if (result.Code == (int)GeneralStatusEnum.Success)
                {
                    if(result.User.Email.StartsWith("srusr-"))
                    {
                        UserModel userModel = (UserModel)await this._popupService.ShowPopupAsync<EmailInputPopUpViewModel>(CancellationToken.None);
                        AttachUserData(userModel.Id, userModel.FirstName, userModel.PhoneNumber, userModel.Email, App.JwtToken);
                        await Navigation.PushAsync<HomePage>();
                    }
                    else
                    {
                        await Navigation.PushAsync<HomePage>();
                    }

                }
                else
                {
                    if (result.Message == MessageConstants.Unsuccess)
                    {
                        throw new Exception(MessageConstants.GeneralError);
                    }
                    else
                    {
                        throw new Exception(result.Message);
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
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
        private async void GoToForgotPassPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();
                await Navigation.PushAsync<ForgotPasswordPage>();
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

        public void AttachUserData(string userId, string userFirstName, string userPhone, string userEmail, string userJwt)
        {
            App.JwtToken = userJwt;
            App.UserId = userId;
            App.UserName = userFirstName;
            App.Phone = userPhone;
            App.Email = userEmail;
        }

        public async Task AutoLogin()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                var userId = App.UserId;
                var jwt = App.JwtToken;

                var httpClient = HttpClientFactory.Create();
                var authResult = await this._authenticationService.Authenticate(userId, jwt, httpClient);

                if (authResult != null && !string.IsNullOrEmpty(authResult.Id) && authResult.Email.StartsWith("srusr-") == false)
                {
                    AttachUserData(authResult.Id, authResult.FirstName, authResult.PhoneNumber, authResult.Email, jwt);
                    await Navigation.PushAsync<HomePage>();
                }
                else
                {
                    UserModel userModel = (UserModel)await this._popupService.ShowPopupAsync<EmailInputPopUpViewModel>(CancellationToken.None);
                    AttachUserData(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                //HideLoading();
                IsBusy = false;
            }
        }

        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (string.IsNullOrEmpty(this.Email))
            {
                this.EmailError = true;
                this.IsError = true;
                res.IsValid = false;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                this.PasswordError = true;
                this.IsError = true;
                res.IsValid = false;
            }

            return res;
        }

        public bool CheckIfUserIsLogged()
        {
            return !string.IsNullOrEmpty(App.JwtToken);
        }

        #endregion
    }
}
