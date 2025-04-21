using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Dynamsoft.License.Maui;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Insurances.Casco;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using Microsoft.Extensions.Configuration;

namespace CovrMe.ViewModels.Pages.Insurances
{
    public partial class ScanOrFillVehicleDataPageViewModel : BaseViewModel, ILicenseVerificationListener, IQueryAttributable
    {

        #region Feilds
        private InsuranceTypeEnum _insuranceType;
        private IConfiguration _configuration;
        #endregion

        #region Props
        public string InsuranceType
        {
            get
            {
                if (_insuranceType == InsuranceTypeEnum.Civil)
                {
                    return "Гражданска отговорност";
                }

                if (_insuranceType == InsuranceTypeEnum.Casco)
                {
                    return "Автокаско";
                }

                return String.Empty;
            }
        }
        #endregion

        public ScanOrFillVehicleDataPageViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
            var dynamsoftAPiKey = _configuration.GetSection("Dynamsoft:LicenseKey").Value;
            LicenseManager.InitLicense(dynamsoftAPiKey, this);
        }

        [RelayCommand]
        public async void GoToSmallCameraPage()
        {
            bool hasCameraPage = this.Navigation.NavigationStack.Any(x => x is CameraPage);

            if (hasCameraPage)
            {
                this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is CameraPage)));
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var parameters = new Dictionary<string, object>
                {
                      { "insuranceType", _insuranceType },
                      { "documentType", DocumentTypeEnum.Small }
                };
                await Navigation.PushAsync<CameraPage>(parameters);
            });
        }

        [RelayCommand]
        public async void GoToBigCameraPage()
        {
            bool hasCameraPage = this.Navigation.NavigationStack.Any(x => x is CameraPage);

            if (hasCameraPage)
            {
                this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is CameraPage)));
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var parameters = new Dictionary<string, object>
                {
                      { "insuranceType", _insuranceType },
                      { "documentType", DocumentTypeEnum.Big }
                };
                await Navigation.PushAsync<CameraPage>(parameters);
            });
        }

        [RelayCommand]

        public async void GoToHandFillPage()
        {
            if (_insuranceType == InsuranceTypeEnum.Civil)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var parameters = new Dictionary<string, object>
                    {
                        { "insuranceType", _insuranceType }
                    };

                    await Navigation.PushAsync<CivilInsuranceVehiclePage>(parameters);
                });
            }

            if (_insuranceType == InsuranceTypeEnum.Casco)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var parameters = new Dictionary<string, object>
                    {
                        { "insuranceType", _insuranceType }
                    };

                    await Navigation.PushAsync<CascoInsuranceVehiclePage>(parameters);
                });
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            this._insuranceType = (InsuranceTypeEnum)query.FirstOrDefault(x => x.Key == "insuranceType").Value;
        }

        public void OnLicenseVerified(bool isSuccess, string message)
        {
            if (!isSuccess)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync<HomePage>();
                    UserDialogs.Instance.ShowToast("Scanner license is expired!");
                });
            }
        }
    }
}
