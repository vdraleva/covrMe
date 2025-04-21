using CommunityToolkit.Maui.Core;
using CovrMe.Models;
using CovrMe.Services.Contracts;
using CovrMe.ViewModels.Pages;
using CovrMe.Views.Pages;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using System.Globalization;

#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Platform;
using System.Globalization;
#endif

namespace CovrMe
{
    public partial class App : Application
    {
        public const string MESSAGE_OK = "OK";
        public const string MESSAGE_Success = "Успешно!";
        public const string MESSAGE_READY = "Готово";
        public const string MESSAGE_CANCEL = "Назад";
        public const string MESSAGE_HEADER_ERROR = "Грешка!";
        public const string MESSAGE_HEADER_ОК = "Съобщение!";
        public const string MESSAGE_HEADER_ATT = "Внимание!";

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#elif iOS
            handler.PlatformView.BackgroundTintList = UIKit.UIColor.Clear;
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.NativeView.BackgroundTintList = UIKit.UIColor.Clear;
            handler.NativeView.BackgroundColor = UIKit.UIColor.Clear;
#endif
            });
            //MainPage = new AppShell();

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.PrependToMapping("MyCustomization", (handler, view) =>
            {
#if ANDROID
                var locale = new Java.Util.Locale("bg", "BG");
                handler.PlatformView.TextLocale = locale;

                Android.Content.Res.Configuration config = new Android.Content.Res.Configuration();
                config.Locale = locale;
                Java.Util.Locale.SetDefault(Java.Util.Locale.Category.Format, locale);

                var resources = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Resources;
                resources.Configuration.SetLocale(locale);
                resources.UpdateConfiguration(config, resources.DisplayMetrics);           

#elif IOS
                if (handler.PlatformView is UIKit.UITextField textField && textField.InputView is UIKit.UIDatePicker datePicker)
                {

                    var localeIdentifier = "bg_BG";
                    datePicker.Locale = new Foundation.NSLocale(localeIdentifier);
                }
#endif
            });

            var authSer = serviceProvider.GetRequiredService<IAuthenticationService>();
            var popupSer = serviceProvider.GetRequiredService<IPopupService>();
            LoginPageViewModel vm = new LoginPageViewModel(authSer, popupSer);
            var page = new LoginPage(vm);

            MainPage = new NavigationPage(page);
        }

        public async static Task DisplayAlert(string title, string message, string ok)
        {
            await Current.MainPage.DisplayAlert(title, message, ok);

        }

        public async static Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await Current.MainPage.DisplayAlert(title, message, ok, cancel);

        }

        public static string JwtToken
        {
            get => Preferences.Default.Get(nameof(JwtToken), string.Empty);
            set => Preferences.Default.Set(nameof(JwtToken), value);
        }

        public static bool IsLocalhost
        {
            get => Preferences.Default.Get(nameof(IsLocalhost), false);
            set => Preferences.Default.Set(nameof(IsLocalhost), value);
        }
        public static string Email
        {
            get => Preferences.Default.Get(nameof(Email), string.Empty);
            set => Preferences.Default.Set(nameof(Email), value);
        }
        public static string UserId
        {
            get => Preferences.Default.Get(nameof(UserId), string.Empty);
            set => Preferences.Default.Set(nameof(UserId), value);
        }
        public static string UserName
        {
            get => Preferences.Default.Get(nameof(UserName), string.Empty);
            set => Preferences.Default.Set(nameof(UserName), value);
        }

        public static string Countries
        {
            get => Preferences.Default.Get(nameof(Countries), string.Empty);
            set => Preferences.Default.Set(nameof(Countries), value);
        }

        public static string VehicleBrands
        {
            get => Preferences.Default.Get(nameof(VehicleBrands), string.Empty);
            set => Preferences.Default.Set(nameof(VehicleBrands), value);
        }

        public static string Regions
        {
            get => Preferences.Default.Get(nameof(Regions), string.Empty);
            set => Preferences.Default.Set(nameof(Regions), value);
        }
        public static DateTime CalendarStartDate
        {
            get => Preferences.Default.Get(nameof(CalendarStartDate), DateTime.Now);
            set => Preferences.Default.Set(nameof(CalendarStartDate), value);
        }
        public static DateTime CalendarEndDate
        {
            get => Preferences.Default.Get(nameof(CalendarEndDate), DateTime.Now);
            set => Preferences.Default.Set(nameof(CalendarEndDate), value);
        }

        public static string Phone
        {
            get => Preferences.Default.Get(nameof(Phone), string.Empty);
            set => Preferences.Default.Set(nameof(Phone), value);
        }

        public static bool NextInstallment
        {
            get => Preferences.Default.Get(nameof(NextInstallment), false);
            set => Preferences.Default.Set(nameof(NextInstallment), value);
        }

        public static DateTime NewInsuranceStartDate
        {
            get => Preferences.Default.Get(nameof(NewInsuranceStartDate), DateTime.MinValue);
            set => Preferences.Default.Set(nameof(NewInsuranceStartDate), value);
        }

        public static string UserGuiltTypes
        {
            get => Preferences.Default.Get(nameof(UserGuiltTypes), string.Empty);
            set => Preferences.Default.Set(nameof(UserGuiltTypes), value);
        }

        public static string VehicleBodyTypes
        {
            get => Preferences.Default.Get(nameof(VehicleBodyTypes), string.Empty);
            set => Preferences.Default.Set(nameof(VehicleBodyTypes), value);
        }

        public static string VehicleColors
        {
            get => Preferences.Default.Get(nameof(VehicleColors), string.Empty);
            set => Preferences.Default.Set(nameof(VehicleColors), value);
        }

        public static string VehicleEngineTypes
        {
            get => Preferences.Default.Get(nameof(VehicleEngineTypes), string.Empty);
            set => Preferences.Default.Set(nameof(VehicleEngineTypes), value);
        }

        public static int InsuranceType
        {
            get => Preferences.Default.Get(nameof(InsuranceType), 0);
            set => Preferences.Default.Set(nameof(InsuranceType), value);
        }

        public static bool IsGroupInsurance
        {
            get => Preferences.Default.Get(nameof(IsGroupInsurance), false);
            set => Preferences.Default.Set(nameof(IsGroupInsurance), value);
        }
    }


}
