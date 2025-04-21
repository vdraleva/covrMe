using CommunityToolkit.Mvvm.Input;
using CovrMe.Models.Insurances;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages.Insurances.MyThings;
using Maui.Plugins.PageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Pages.Insurances.MyThings
{
    public partial class MyThingsInsuranceCategoryPageViewModel : BaseViewModel
    {
        #region Fields

        private int _propertyType = (int)MyThingsPropertyTypeEnum.Bicycle;
        private int _objectTypeId = (int)MyThingsObjectTypeEnum.Bicycle;

        #endregion

        public MyThingsInsuranceCategoryPageViewModel()
        {

        }

        #region Props

        public int PropertyType
        {
            get { return _propertyType; }
            set { SetProperty(ref _propertyType, value); }
        }

        public int ObjectTypeId
        {
            get { return _objectTypeId; }
            set { SetProperty(ref _objectTypeId, value); }
        }

        #endregion

        #region Commands

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

                var myThingsInfo = new MyThingsInsuranceOfferModel
                {
                    PropertyTypeId = this.PropertyType,
                    ObjectTypeId = this.ObjectTypeId
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"myThingsInfo", myThingsInfo}
                    };

                await Navigation.PushAsync<MyThingsCharacteristicsPage>(parameters);
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
        #endregion
    }
}
