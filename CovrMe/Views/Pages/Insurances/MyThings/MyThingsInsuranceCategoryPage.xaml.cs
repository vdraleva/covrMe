using CovrMe.Shared.Enums;
using CovrMe.ViewModels.Pages.Insurances.MyThings;

namespace CovrMe.Views.Pages.Insurances.MyThings;

public partial class MyThingsInsuranceCategoryPage : ContentPage
{
    public MyThingsInsuranceCategoryPageViewModel viewModel;
    public MyThingsInsuranceCategoryPage(MyThingsInsuranceCategoryPageViewModel vm)
	{
        InitializeComponent();
        vm.Navigation = Navigation;
        viewModel = vm;
        BindingContext = vm;
    }

    private void BycicleRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BycicleRadio.IsChecked)
        {
            viewModel.PropertyType = (int)MyThingsPropertyTypeEnum.Bicycle;
            viewModel.ObjectTypeId = (int)MyThingsObjectTypeEnum.Bicycle;

            TricycleRadio.IsChecked = false;
            ScooterRadio.IsChecked = false;
            GlassesRadio.IsChecked = false;
        }
    }

    private void BycicleText_Tapped(object sender, TappedEventArgs e)
    {
        if (BycicleRadio.IsChecked)
        {
            BycicleRadio.IsChecked = false;
        }
        else
        {
            BycicleRadio.IsChecked = true;
        }
    }

    private void TricycleRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (TricycleRadio.IsChecked)
        {
            viewModel.PropertyType = (int)MyThingsPropertyTypeEnum.Bicycle;
            viewModel.ObjectTypeId = (int)MyThingsObjectTypeEnum.Tricycle;

            BycicleRadio.IsChecked = false;
            ScooterRadio.IsChecked = false;
            GlassesRadio.IsChecked = false;
        }
    }

    private void TricycleText_Tapped(object sender, TappedEventArgs e)
    {
        if (TricycleRadio.IsChecked)
        {
            TricycleRadio.IsChecked = false;
        }
        else
        {
            TricycleRadio.IsChecked = true;
        }
    }

    private void ScooterRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (ScooterRadio.IsChecked)
        {
            viewModel.PropertyType = (int)MyThingsPropertyTypeEnum.Bicycle;
            viewModel.ObjectTypeId = (int)MyThingsObjectTypeEnum.Scooter;

            BycicleRadio.IsChecked = false;
            TricycleRadio.IsChecked = false;
            GlassesRadio.IsChecked = false;
        }
    }

    private void ScooterText_Tapped(object sender, TappedEventArgs e)
    {
        if (ScooterRadio.IsChecked)
        {
            ScooterRadio.IsChecked = false;
        }
        else
        {
            ScooterRadio.IsChecked = true;
        }
    }

    private void GlassesRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (GlassesRadio.IsChecked)
        {
            viewModel.PropertyType = (int)MyThingsPropertyTypeEnum.Glasses;
            viewModel.ObjectTypeId = (int)MyThingsObjectTypeEnum.Glasses;

            BycicleRadio.IsChecked = false;
            TricycleRadio.IsChecked = false;
            ScooterRadio.IsChecked = false;
        }
    }

    private void GlassesText_Tapped(object sender, TappedEventArgs e)
    {
        if (GlassesRadio.IsChecked)
        {
            GlassesRadio.IsChecked = false;
        }
        else
        {
            GlassesRadio.IsChecked = true;
        }
    }
}