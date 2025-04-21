using CommunityToolkit.Mvvm.Input;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Insurances.Casco;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using CovrMe.Views.Pages.Insurances.HealthInsurance;
using CovrMe.Views.Pages.Insurances.Mountain;
using CovrMe.Views.Pages.Insurances.MyThings;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances
{
    public partial class NewInsurancePageViewModel : BaseViewModel
    {
        #region Fields
        #endregion

        public NewInsurancePageViewModel()
        {
            App.IsGroupInsurance = false;
        }

        #region Props
        #endregion

        #region Commands

        [RelayCommand]
        private async void GoToCivilInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                bool hasScanPage = this.Navigation.NavigationStack.Any(x => x is ScanOrFillVehicleDataPage);

                if (hasScanPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is ScanOrFillVehicleDataPage)));
                }

                var parameters = new Dictionary<string, object>
                {
                    {   "insuranceType", InsuranceTypeEnum.Civil }
                };
                await Navigation.PushAsync<ScanOrFillVehicleDataPage>(parameters);
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
        private async void GoToTravelInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                bool hasThisPage = this.Navigation.NavigationStack.Any(x => x is TravelInsuranceLocationPage);

                if (hasThisPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is TravelInsuranceLocationPage)));
                }

                await Navigation.PushAsync<TravelInsuranceLocationPage>();
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
        private async void GoToMountainInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                bool hasThisPage = this.Navigation.NavigationStack.Any(x => x is MountainInsuranceInsuredUsersPage);

                if (hasThisPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is MountainInsuranceInsuredUsersPage)));
                }

                await Navigation.PushAsync<MountainInsuranceInsuredUsersPage>();
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
        private async void GoToHealthInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                bool hasThisPage = this.Navigation.NavigationStack.Any(x => x is HealthInsuranceInsuredUsersPage);

                if (hasThisPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is HealthInsuranceInsuredUsersPage)));
                }

                await Navigation.PushAsync<HealthInsuranceInsuredUsersPage>();
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
        private async void GoToMyThingsPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                bool hasThisPage = this.Navigation.NavigationStack.Any(x => x is MyThingsInsuranceCategoryPage);

                if (hasThisPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is MyThingsInsuranceCategoryPage)));
                }

                await Navigation.PushAsync<MyThingsInsuranceCategoryPage>();
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
        private async void GoToCascoInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                bool hasScanPage = this.Navigation.NavigationStack.Any(x => x is ScanOrFillVehicleDataPage);

                if (hasScanPage)
                {
                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is ScanOrFillVehicleDataPage)));
                }

                var parameters = new Dictionary<string, object>
                {
                    {   "insuranceType", InsuranceTypeEnum.Casco }
                };
                await Navigation.PushAsync<ScanOrFillVehicleDataPage>(parameters);
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
    }
}
