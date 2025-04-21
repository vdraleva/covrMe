using MvvmHelpers;
using System.Net.Mail;
using CovrMe.Shared.Constants;
using CovrMe.Models;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Input;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Enums;
using CovrMe.Factories;
using CovrMe.Shared;
using CovrMe.Views.Pages;
using Maui.Plugins.PageResolver;


namespace CovrMe.ViewModels.Pages
{
    public partial class RegisterPageViewModel : BaseViewModel
    {
        #region Fields
        private string firstName;
        private string surName;
        private string lastName;
        private string uin;
        private string phone;
        private string address;
        private string email;
        private string password;                
        private string confirmPassword;
        private string vatNumber;
        private string phoneNumberCode;
        private string companyName;

        private bool isError = false;

        private bool passwordError;
        private bool emailError;
        private bool confirmPasswordError;
        private bool firstNameError;
        private bool lastNameError;
        private bool surNameError;
        private bool phoneError;
        private bool addressError;
        private bool uinError;
        private bool vatError;
        private bool companyNameError;

        private IAuthenticationService _authenticationService;

        #endregion

        public RegisterPageViewModel(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        #region Props

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

        public bool ConfirmPasswordError
        {
            get { return confirmPasswordError; }
            set
            {
                if (confirmPasswordError != value)
                {
                    confirmPasswordError = value;
                    OnPropertyChanged(nameof(ConfirmPasswordError));
                }
            }
        }

        public bool FirstNameError
        {
            get { return firstNameError; }
            set
            {
                if (firstNameError != value)
                {
                    firstNameError = value;
                    OnPropertyChanged(nameof(FirstNameError));
                }
            }
        }

        public bool LastNameError
        {
            get { return lastNameError; }
            set
            {
                if (lastNameError != value)
                {
                    lastNameError = value;
                    OnPropertyChanged(nameof(LastNameError));
                }
            }
        }

        public bool SurNameError
        {
            get { return surNameError; }
            set
            {
                if (surNameError != value)
                {
                    surNameError = value;
                    OnPropertyChanged(nameof(SurNameError));
                }
            }
        }

        public bool PhoneError
        {
            get { return phoneError; }
            set
            {
                if (phoneError != value)
                {
                    phoneError = value;
                    OnPropertyChanged(nameof(PhoneError));
                }
            }
        }

        public bool AddressError
        {
            get { return addressError; }
            set
            {
                if (addressError != value)
                {
                    addressError = value;
                    OnPropertyChanged(nameof(AddressError));
                }
            }
        }

        public bool UinError
        {
            get { return uinError; }
            set
            {
                if (uinError != value)
                {
                    uinError = value;
                    OnPropertyChanged(nameof(UinError));
                }
            }
        }

        public bool VatError
        {
            get { return vatError; }
            set
            {
                if (vatError != value)
                {
                    vatError = value;
                    OnPropertyChanged(nameof(VatError));
                }
            }
        }

        public bool CompanyNameError
        {
            get { return companyNameError; }
            set
            {
                if (companyNameError != value)
                {
                    companyNameError = value;
                    OnPropertyChanged(nameof(CompanyNameError));
                }
            }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async void Register()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                var valResult = this.ValidateInput();

                if (!valResult.IsValid)
                {
                    throw new Exception(valResult.Message);
                }

                var httpClient = HttpClientFactory.Create();


                var regResult = await this._authenticationService.Register(this.Email.TrimEnd(), this.Password.TrimEnd(), httpClient);
        
                if (regResult.Code == (int)GeneralStatusEnum.Success)
                {
                    await Navigation.PushAsync<SuccessfulRegistrationPage>();
                }
                else
                {
                    if (!string.IsNullOrEmpty(regResult.Message))
                    {
                        throw new Exception(regResult.Message);
                    }

                    throw new Exception(MessageConstants.GeneralError);
                }
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
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
        private async void GoToInsuranceBgTerms(object obj)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                ShowLoading();
                string uri = GlobalConstants.ObshtiUsloviaURL;
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

        private ValidationModel ValidateInput()
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

                if (!this.ValidateEmail())
                {
                    this.EmailError = true;
                    this.IsError = true;
                    res.IsValid = false;
                    res.Message = MessageConstants.InvalidEmail;
                }
            }

            return res;
        }

        private bool PasswordsMatch()
        {
            return this.ConfirmPassword == this.Password;
        }

        private bool ValidateBulgarian(string input)
        {
            if (Regex.IsMatch(input, GlobalConstants.BulgarianNameRegex))
            {
                return true;
            }

            return false;
        }

        private bool ValidateEmail()
        {
            if (Regex.IsMatch(this.Email.TrimEnd(), GlobalConstants.EmailRegex))
            {
                return true;
            }

            return false;
        }

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

        #endregion
    }
}
