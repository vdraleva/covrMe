using CommunityToolkit.Mvvm.Input;
using CovrMe.Models;
using CovrMe.Models.Insurances;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CovrMe.Views.ContentViews;

public partial class InsuredUsersContentView : ContentView
{
    public static readonly BindableProperty AgesListProperty = BindableProperty.Create(
        nameof(AgesList), typeof(ObservableCollection<InsuredUsersAgeTemplateModel>), typeof(InsuredUsersContentView), null);

    public static readonly BindableProperty HeightPackageIOSProperty = BindableProperty.Create(
        nameof(HeightPackageIOS), typeof(int), typeof(InsuredUsersContentView), 0);

    private bool _isBusy;

    public InsuredUsersContentView()
	{
		InitializeComponent();
        BindingContext = this;
        this.AgesList = new ObservableCollection<InsuredUsersAgeTemplateModel>();
        HeightPackageIOS = 100;
    }

    public ObservableCollection<InsuredUsersAgeTemplateModel> AgesList
    {
        get { return (ObservableCollection<InsuredUsersAgeTemplateModel>)GetValue(AgesListProperty); }
        set { SetValue(AgesListProperty, value); }
    }
    public int HeightPackageIOS
    {
        get { return (int)GetValue(HeightPackageIOSProperty); }
        set { SetValue(HeightPackageIOSProperty, value); }
    }


    [RelayCommand]
    private async void Plus()
    {
        try
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            var cuurentCount = this.AgesList.Count;
            var userAgeModel = new InsuredUsersAgeTemplateModel();

            userAgeModel.Index = cuurentCount;
            userAgeModel.Number = cuurentCount + 1;

            AgesList.Add(userAgeModel);
            HeightPackageIOS = 80 * AgesList.Count();
        }
        catch (Exception ex)
        {
            await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
        }
        finally
        {
            _isBusy = false;
        }
    }

    [RelayCommand]
    private async void Minus()
    {
        try
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            if (AgesList.Count != 0)
            {
                var element = this.AgesList.LastOrDefault();
                AgesList.Remove(element);

                HeightPackageIOS = 80 * AgesList.Count();
            }
        }
        catch (Exception ex)
        {
            await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
        }
        finally
        {
            _isBusy = false;
        }
    }

}