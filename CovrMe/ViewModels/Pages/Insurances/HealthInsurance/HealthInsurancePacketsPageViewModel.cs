using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Shared.Constants;
using CovrMe.Views.Pages.Insurances.HealthInsurance;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.HealthInsurance
{
    public partial class HealthInsurancePacketsPageViewModel : BaseViewModel, IQueryAttributable
    {
        #region Fields
        private bool _prestigeCardOpened;
        private bool _comforteCardOpened;

        private bool _prestigePacketSelected;
        private bool _comfortPacketSelected;
        private int _packetId = 8;


        private List<InsuredUsersAgeTemplateModel> _usersAge;
        private HealthInsuranceOfferModel _healthInfo;
        #endregion

        public HealthInsurancePacketsPageViewModel()
        {
            PrestigePacketSelected = true;
            PrestigeCardOpened = true;

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("bg-BG");
            string date = DateTime.Now.ToString("MMMM", culture);
            string upperDate = char.ToUpper(date[0]) + date.Substring(1);
        }

        #region Props
        public bool PrestigeCardOpened
        {
            get { return _prestigeCardOpened; }
            set { SetProperty(ref _prestigeCardOpened, value); }
        }
        public bool ComforteCardOpened
        {
            get { return _comforteCardOpened; }
            set { SetProperty(ref _comforteCardOpened, value); }
        }

        public bool PrestigePacketSelected
        {
            get { return _prestigePacketSelected; }
            set { SetProperty(ref _prestigePacketSelected, value); }
        }

        public bool ComfortPacketSelected
        {
            get { return _comfortPacketSelected; }
            set { SetProperty(ref _comfortPacketSelected, value); }
        }

        public int PacketId
        {
            get { return _packetId; }
            set { SetProperty(ref _packetId, value); }
        }

        #endregion

        #region Command

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

                this._healthInfo.PacketId = this.PacketId;
                var parameters = new Dictionary<string, object>
                    {
                        {"userAgeList", this._usersAge},
                        {"healthInfo", _healthInfo}
                    };


                await Navigation.PushAsync<HealthInsurancePeriodPage>(parameters);
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count == 0)
            {
                return;
            }

            _healthInfo = query.FirstOrDefault(x => x.Key == "healthInfo").Value as HealthInsuranceOfferModel;
            _usersAge = query.FirstOrDefault(x => x.Key == "userAgeList").Value as List<InsuredUsersAgeTemplateModel>;

        }

        #endregion
    }
}
