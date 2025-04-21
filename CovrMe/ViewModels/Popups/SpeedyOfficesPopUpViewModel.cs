using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Deliveries.Result;
using CovrMe.Models.Locations.Result;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Popups
{
    public partial class SpeedyOfficesPopUpViewModel : BaseViewModel
    {
        #region Fields

        private double _displayWidth;
        private ObservableCollection<SpeedyOfficeModel> _officeCollection;

        #endregion

        public SpeedyOfficesPopUpViewModel()
        {
            this.OfficeCollection = new ObservableCollection<SpeedyOfficeModel>();

            SetScreenWidth();
        }

        #region Collections
        public ObservableCollection<SpeedyOfficeModel> OfficeCollection
        {
            get => _officeCollection;
            set => SetProperty(ref _officeCollection, value);
        }
        #endregion

        #region Props

        public double DisplayWidth
        {
            get { return this._displayWidth; }
            set { SetProperty(ref this._displayWidth, value); }
        }

        #endregion

        #region Methods

        public async Task SelectOffice(int officeId)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;

                foreach (var item in this.OfficeCollection)
                {
                    item.IsChecked = false;

                    if (item.Id == officeId)
                    {
                        item.IsChecked = true;
                    }
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
        public async Task PopulateCollection(List<SpeedyOfficeModel> offices)
        {
            var officesCol = new ObservableCollection<SpeedyOfficeModel>();
            this.OfficeCollection.Clear();

            foreach (var office in offices)
            {
                officesCol.Add(office);
            }

            this.OfficeCollection = officesCol;
        }
        private void SetScreenWidth()
        {
            this.DisplayWidth = (DeviceDisplay.MainDisplayInfo.Width / 2) - 200;
        }

        #endregion

    }
}
