using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages
{
    public partial class ResetPasswordPageViewModel : BaseViewModel
    {
        #region Fields

        private string password;
        private string confirmPassword;
        private bool isValid = true;

        private bool isError = false;
        private bool passwordError = false;
        private bool confirmPasswordError = false;
        private IAuthenticationService _authenticationService;

        #endregion

        public ResetPasswordPageViewModel(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        #region Props

        public string Password
        {
            get { return this.password; }
            set { SetProperty(ref this.password, value); }
        }
        public string ConfirmPassword
        {
            get { return this.confirmPassword; }
            set { SetProperty(ref this.confirmPassword, value); }
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

        public bool PasswordError
        {
            get { return passwordError; }
            set
            {
                if (passwordError != value)
                {
                    isError = value;
                    OnPropertyChanged(nameof(PasswordError));
                }
            }
        }

        public bool ConfirmPasswordError
        {
            get { return confirmPasswordError; }
            set
            {
                if (confirmPasswordError != value)
                {
                    isError = value;
                    OnPropertyChanged(nameof(ConfirmPasswordError));
                }
            }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async Task ChangePassword()
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
                    throw new InvalidOperationException(validateInput.Message);
                }
              
                var email = App.Email;
                App.Email = string.Empty;

                var httpClient = HttpClientFactory.Create();
                var resetPasswordResult = await this._authenticationService.ResetUserPassword(email, this.Password, httpClient);

                if(resetPasswordResult.Code == (int)GeneralStatusEnum.Success)
                {
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    throw new Exception(MessageConstants.GeneralError);
                }                
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                this.Password = string.Empty;
                this.ConfirmPassword = string.Empty;
                IsBusy = false;
                HideLoading();
            }
        }

        #endregion

        #region Methods

        private ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (string.IsNullOrEmpty(this.Password))
            {
                this.PasswordError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredPassword;
            }

            if (string.IsNullOrEmpty(this.ConfirmPassword))
            {
                this.ConfirmPasswordError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredConfirmPassword;
            }

            if (res.IsValid)
            {
                if (!this.PasswordsMatch())
                {
                    this.PasswordError = true;
                    this.ConfirmPasswordError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.PassNotMatch;
                }
            }

            return res;
        }

        private bool PasswordsMatch()
        {
            return ConfirmPassword == Password;
        }
        #endregion
    }
}
