using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Models;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;
using System.Net.Mail;

namespace CovrMe.ViewModels.Pages
{
    public partial class ForgotPasswordViewModel : BaseViewModel
    {
        #region Fields

        private string email;
        private bool emailError = false;
        private bool isError = false;
        private IAuthenticationService _authenticationService;

        #endregion

        public ForgotPasswordViewModel(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        #region Props

        public string Email
        {
            get { return this.email; }
            set { SetProperty(ref this.email, value); }
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

        #endregion

        #region Commands

        [RelayCommand]
        private async Task SendEmail()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();

                this.Email = this.Email.TrimEnd();

                var valResult = this.ValidateInput();

                if (!valResult.IsValid)
                {
                    throw new Exception(valResult.Message);
                }

                var httpClient = HttpClientFactory.Create();
                var sendEmailResult = await this._authenticationService.SendEmailWithCodeForgottenPassword(this.Email, httpClient);

                if (sendEmailResult.Code == (int)GeneralStatusEnum.Success)
                {
                    App.Email = this.Email;
                    await Navigation.PushAsync<OTPPage>();
                }
                else
                {
                   
                    throw new Exception(sendEmailResult.Message);
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

        #endregion


        #region Methods

        ValidationModel ValidateInput()
        {
            var res = new ValidationModel();
            res.IsValid = true;

            if (string.IsNullOrEmpty(this.Email))
            {
                this.EmailError = true;
                this.IsError = true;
                res.IsValid = false;
                res.Message = MessageConstants.RequiredEmail;
            }
            else
            {
                if (!this.EmailValidation())
                {
                    this.EmailError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.InvalidEmail;
                }
            }

            return res;
        }

        private bool EmailValidation()
        {
            try
            {
                MailAddress m = new MailAddress(this.Email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion
    }
}
