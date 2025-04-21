using CommunityToolkit.Mvvm.Input;
using CovrMe.Factories;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;
namespace CovrMe.ViewModels.Pages
{
    public partial class OTPPageViewModel : BaseViewModel
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;

        private string _digit1;
        private string _digit2;
        private string _digit3;
        private string _digit4;
        private bool isValid = true;

        #endregion

        #region Props
        public string Digit1
        {
            get { return _digit1; }
            set
            {
                _digit1 = value;
                OnPropertyChanged(nameof(Digit1));
            }
        }
        public string Digit2
        {
            get { return _digit2; }
            set
            {
                _digit2 = value;
                OnPropertyChanged(nameof(Digit2));
            }
        }
        public string Digit3
        {
            get { return _digit3; }
            set
            {
                _digit3 = value;
                OnPropertyChanged(nameof(Digit3));
            }
        }
        public string Digit4
        {
            get { return _digit4; }
            set
            {
                _digit4 = value;
                OnPropertyChanged(nameof(Digit4));
            }
        }

        #endregion
        public OTPPageViewModel(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }
        #region Commands

        [RelayCommand]
        private async Task VerifyCode()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                ShowLoading();

                var verificationCode = GetVerificationCode();

                var httpClient = HttpClientFactory.Create();

                var result = await _authenticationService.CheckResetPasswordCode(App.Email, verificationCode, httpClient);
                if (result.IsCodeValid)
                {
                    await Navigation.PushAsync<ResetPasswordPage>();
                }
                else
                {
                    throw new Exception(MessageConstants.InvalidConfirmCode);
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
                this.Digit1 = string.Empty;
                this.Digit2 = string.Empty;
                this.Digit3 = string.Empty;
                this.Digit4 = string.Empty;
            }
        }

        #endregion

        #region Methods

        public string GetVerificationCode()
        {
            return Digit1 + Digit2 + Digit3 + Digit4;
        }

        #endregion
    }
}
