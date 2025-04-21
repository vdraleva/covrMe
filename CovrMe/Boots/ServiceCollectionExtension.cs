using CommunityToolkit.Maui;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Services.Implementation;
using CovrMe.ViewModels.ContentViews;
using CovrMe.ViewModels.Pages;
using CovrMe.ViewModels.Pages.Insurances;
using CovrMe.ViewModels.Pages.Insurances.Casco;
using CovrMe.ViewModels.Pages.Insurances.CivilInsurance;
using CovrMe.ViewModels.Pages.Insurances.HealthInsurance;
using CovrMe.ViewModels.Pages.Insurances.Mountain;
using CovrMe.ViewModels.Pages.Insurances.MyThings;
using CovrMe.ViewModels.Pages.Insurances.TravelInsurance;
using CovrMe.ViewModels.Pages.Payment;
using CovrMe.ViewModels.Pages.Profile;
using CovrMe.ViewModels.Pages.Speedy;
using CovrMe.ViewModels.Popups;
using CovrMe.Views.ContentViews;
using CovrMe.Views.Pages;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Insurances.Casco;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using CovrMe.Views.Pages.Insurances.HealthInsurance;
using CovrMe.Views.Pages.Insurances.Mountain;
using CovrMe.Views.Pages.Insurances.MyThings;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using CovrMe.Views.Pages.Payment;
using CovrMe.Views.Pages.Profile;
using CovrMe.Views.Pages.Speedy;
using CovrMe.Views.Popups;

namespace CovrMe.Boots
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services, Settings.Settings settings)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>(i =>
                new AuthenticationService(settings.BackendServiceEndpoint));

            services.AddTransient<IUserService, UserService>(i =>
                new UserService(settings.BackendServiceEndpoint));

            services.AddTransient<IVehicleService, VehicleService>(i =>
                new VehicleService(settings.BackendServiceEndpoint));

            services.AddTransient<ILocationService, LocationService>(i =>
                new LocationService(settings.BackendServiceEndpoint));

            services.AddTransient<IInsuranceService, InsuranceService>(i =>
                new InsuranceService(settings.BackendServiceEndpoint));

            services.AddTransient<ICacheService, CacheService>(i =>
                new CacheService(settings.BackendServiceEndpoint));

            services.AddTransient<IDeliveryService, DeliveryService>(i =>
                new DeliveryService(settings.BackendServiceEndpoint));

            services.AddTransient<IPaymentService, PaymentService>(i =>
                new PaymentService(settings.BackendServiceEndpoint));
            services.AddTransient<IRegExtractorRegexService, RegExtractorRegexService>();
            services.AddTransient<IOcrService, OcrService>();
            services.AddTransient<RegCertificateResultModel>();
        }

        public static void RegisterViewsAndroid(this IServiceCollection services)
        {
            services.AddTransient<LoginPage, LoginPageViewModel>();
            services.AddTransient<RegisterPage, RegisterPageViewModel>();
            services.AddTransient<HomePage, HomePageViewModel>();
            services.AddTransient<ForgotPasswordPage, ForgotPasswordViewModel>();
            services.AddTransient<OTPPage, OTPPageViewModel>();
            services.AddTransient<ResetPasswordPage, ResetPasswordPageViewModel>();
            services.AddTransient<CivilInsuranceVehiclePage, CivilInsuranceVehicleViewModel>();
            services.AddTransient<CivilInsuranceOwnerPage, CivilInsuranceOwnerViewModel>();
            services.AddTransient<CivilInsuranceUsualDriverPage, CivilInsuranceUsualDriverViewModel>();
            services.AddTransient<CivilInsuranceOffersPage, CivilInsuranceOffersViewModel>();
            services.AddTransient<CivilInsuranceCalendarPage, CivilInsuranceCalendarPageViewModel>();
            services.AddTransient<CivilInsuranceSummaryPage, CivilInsuranceSummaryViewModel>();
            services.AddTransient<CivilInsuranceDocumentsPage, CivilInsuranceDocumentsPageViewModel>();
            services.AddTransient<SpeedyChooseDeliveryPage, SpeedyChooseDeliveryViewModel>();
            services.AddTransient<SpeedyDeliveryOfficePage, SpeedyDeliveryOfficeViewModel>();
            services.AddTransient<SpeedyDeliveryUserAddressPage, SpeedyDeliveryUserAddressViewModel>();
            services.AddTransient<ThankYouPage, ThankYouPageViewModel>();
            services.AddTransient<PaymentPage, PaymentPageViewModel>();
            services.AddTransient<ProfilePage, ProfilePageViewModel>();
            services.AddTransient<PersonalDataPage, PersonalDataViewModel>();
            services.AddTransient<FamilyFriendsDataPage, FamilyFriendsDataPageViewModel>();
            services.AddTransient<CarDataPage, CarDataPageViewModel>();
            services.AddTransient<MyInsurancesPage, MyInsurancesPageViewModel>();
            services.AddTransient<ExistingCivilInsurancePage, ExistingCivilInsurancePageViewModel>();
            services.AddTransient<ErrorPage, ErrorPageViewModel>();
            services.AddTransient<TravelInsuranceLocationPage, TravelInsuranceLocationViewModel>();
            services.AddTransient<TravelInsuranceInsuredUsersPage, TravelInsuranceInsuredUsersViewModel>();
            services.AddTransient<TravelInsuranceCalendarPage, TravelInsuranceCalendarViewModel>();
            services.AddTransient<TravelInsuranceOffersPage, TravelInsuranceOffersViewModel>();
            services.AddTransient<TravelInsuranceInfoPage, TravelInsuranceInfoPageViewModel>();
            services.AddTransient<TravelInsuranceOwnerPage, TravelInsuranceOwnerViewModel>();
            services.AddTransient<TravelInsuranceUserInfoPage, TravelInsuranceUserInfoViewModel>();
            services.AddTransient<TravelInsuranceSummaryPage, TravelInsuranceSummaryViewModel>();
            services.AddTransient<TravelInsuranceDocumentsPage, TravelInsuranceDocumentsViewModel>();
            services.AddTransient<PrePaymentPage, PrePaymentPageViewModel>();
            services.AddTransient<MountainInsuranceInsuredUsersPage, MountainInsuranceInsuredUsersViewModel>();
            services.AddTransient<MountainInsuranceCalendarPage, MountainInsuranceCalendarViewModel>();
            services.AddTransient<MountainInsuranceOffersPage, MountainInsuranceOffersViewModel>();
            services.AddTransient<MountainInsuranceInfoPage, MountainInsuranceInfoPageViewModel>();
            services.AddTransient<MountainInsuranceSummaryPage, MountainInsuranceSummaryViewModel>();
            services.AddTransient<MountainInsuranceDocumentsPage, MountainInsuranceDocumentsViewModel>();
            services.AddTransient<InsuredUsersPage, InsuredUsersPageViewModel>();
            services.AddTransient<HealthInsuranceInsuredUsersPage, HealthInsuranceInsuredUsersViewModel>();
            services.AddTransient<HealthInsurancePacketsPage, HealthInsurancePacketsPageViewModel>();
            services.AddTransient<HealthInsurancePeriodPage, HealthInsurancePeriodPageViewModel>();
            services.AddTransient<HealthInsuranceOffersPage, HealthInsuranceOffersPageViewModel>();
            services.AddTransient<HealthDeclarationPage, HealthDeclarationPageViewModel>();
            services.AddTransient<InsuranceOwnerPage, InsuranceOwnerPageViewModel>();
            services.AddTransient<HealthInsuranceDocumentsPage, HealthInsuranceDocumentsPageViewModel>();
            services.AddTransient<HealthInsuranceSummaryPage, HealthInsuranceSummaryPageViewModel>();
            services.AddTransient<MyThingsCharacteristicsPage, MyThingsCharacteristicsPageViewModel>();
            services.AddTransient<MyThingsInsuranceCategoryPage, MyThingsInsuranceCategoryPageViewModel>();
            services.AddTransient<MyThingsInsuranceCalendarPage, MyThingsInsuranceCalendarPageViewModel>();
            services.AddTransient<MyThingsInsuranceOffersPage, MyThingsInsuranceOffersViewModel>();
            services.AddTransient<MyThingsInsuranceInfoPage, MyThingsInsuranceInfoPageViewModel>();
            services.AddTransient<MyThingsInsuranceDocumentsPage, MyThingsInsuranceDocumentsPageViewModel>();
            services.AddTransient<TravelInsuredUsersPage, TravelInsuredUsersPageViewModel>();
            services.AddTransient<NewInsurancePage, NewInsurancePageViewModel>();
            services.AddTransient<SuccessfulRegistrationPage, SuccessfulRegistrationPageViewModel>();
            services.AddTransient<CivilInsuranceVehicleAdditionalInfoPage, CivilInsuranceVehicleAdditionalInfoViewModel>();
            services.AddTransient<CivilInsuranceLongOffersPage, CivilInsuranceLongOffersViewModel>();
            services.AddTransient<CascoInsuranceVehiclePage, CascoInsuranceVehicleViewModel>();
            services.AddTransient<CascoInsuranceOwnerPage, CascoInsuranceOwnerViewModel>();
            services.AddTransient<CascoInsuranceThankYouPage, CascoInsuranceThankYouViewModel>();
            services.AddTransient<ScanOrFillVehicleDataPage, ScanOrFillVehicleDataPageViewModel>();
            services.AddTransient<CameraPage>();

            //Popups
            services.AddTransientPopup<EmailInputPopUp, EmailInputPopUpViewModel>();
            services.AddTransientPopup<SpeedyOfficesPopUp, SpeedyOfficesPopUpViewModel>();
            services.AddTransientPopup<MountainXtreemInfoPopUp, MountainXtreemInfoPopUpViewModel>();
            services.AddTransientPopup<HealthInsuranceContactUsPopUp, HealthInsuranceContactUsPopUpViewModel>();
            services.AddTransientPopup<CameraGuidePopUp, CameraGuidePopUpViewModel>();
        }

        public static void RegisterViewsiOS(this IServiceCollection services)
        {
            services.AddTransient<LoginPageViewModel>().AddTransient<LoginPage>();
            services.AddTransient<RegisterPageViewModel>().AddTransient<RegisterPage>();
            services.AddTransient<HomePageViewModel>().AddTransient<HomePage>();
            services.AddTransient<ForgotPasswordViewModel>().AddTransient<ForgotPasswordPage>();
            services.AddTransient<OTPPageViewModel>().AddTransient<OTPPage>();
            services.AddTransient<ResetPasswordPageViewModel>().AddTransient<ResetPasswordPage>();
            services.AddTransient<CivilInsuranceVehicleViewModel>().AddTransient<CivilInsuranceVehiclePage>();
            services.AddTransient<CivilInsuranceOwnerViewModel>().AddTransient<CivilInsuranceOwnerPage>();
            services.AddTransient<CivilInsuranceUsualDriverViewModel>().AddTransient<CivilInsuranceUsualDriverPage>();
            services.AddTransient<CivilInsuranceOffersViewModel>().AddTransient<CivilInsuranceOffersPage>();
            services.AddTransient<CivilInsuranceCalendarPageViewModel>().AddTransient<CivilInsuranceCalendarPage>();
            services.AddTransient<CivilInsuranceSummaryViewModel>().AddTransient<CivilInsuranceSummaryPage>();
            services.AddTransient<CivilInsuranceDocumentsPageViewModel>().AddTransient<CivilInsuranceDocumentsPage>();
            services.AddTransient<SpeedyChooseDeliveryViewModel>().AddTransient<SpeedyChooseDeliveryPage>();
            services.AddTransient<SpeedyDeliveryOfficeViewModel>().AddTransient<SpeedyDeliveryOfficePage>();
            services.AddTransient<SpeedyDeliveryUserAddressViewModel>().AddTransient<SpeedyDeliveryUserAddressPage>();
            services.AddTransient<ThankYouPageViewModel>().AddTransient<ThankYouPage>();
            services.AddTransient<PaymentPageViewModel>().AddTransient<PaymentPage>();
            services.AddTransient<ProfilePageViewModel>().AddTransient<ProfilePage>();
            services.AddTransient<PersonalDataViewModel>().AddTransient<PersonalDataPage>();
            services.AddTransient<FamilyFriendsDataPageViewModel>().AddTransient<FamilyFriendsDataPage>();
            services.AddTransient<CarDataPageViewModel>().AddTransient<CarDataPage>();
            services.AddTransient<MyInsurancesPageViewModel>().AddTransient<MyInsurancesPage>();
            services.AddTransient<ExistingCivilInsurancePageViewModel>().AddTransient<ExistingCivilInsurancePage>();
            services.AddTransient<ErrorPageViewModel>().AddTransient<ErrorPage>();
            services.AddTransient<TravelInsuranceLocationViewModel>().AddTransient<TravelInsuranceLocationPage>();
            services.AddTransient<TravelInsuranceInsuredUsersViewModel>().AddTransient<TravelInsuranceInsuredUsersPage>();
            services.AddTransient<TravelInsuranceCalendarViewModel>().AddTransient<TravelInsuranceCalendarPage>();
            services.AddTransient<TravelInsuranceOffersViewModel>().AddTransient<TravelInsuranceOffersPage>();
            services.AddTransient<TravelInsuranceInfoPageViewModel>().AddTransient<TravelInsuranceInfoPage>();
            services.AddTransient<TravelInsuranceOwnerViewModel>().AddTransient<TravelInsuranceOwnerPage>();
            services.AddTransient<TravelInsuranceUserInfoViewModel>().AddTransient<TravelInsuranceUserInfoPage>();
            services.AddTransient<TravelInsuranceSummaryViewModel>().AddTransient<TravelInsuranceSummaryPage>();
            services.AddTransient<TravelInsuranceDocumentsViewModel>().AddTransient<TravelInsuranceDocumentsPage>();
            services.AddTransient<PrePaymentPageViewModel>().AddTransient<PrePaymentPage>();
            services.AddTransient<MountainInsuranceInsuredUsersViewModel>().AddTransient<MountainInsuranceInsuredUsersPage>();
            services.AddTransient<MountainInsuranceCalendarViewModel>().AddTransient<MountainInsuranceCalendarPage>();
            services.AddTransient<MountainInsuranceOffersViewModel>().AddTransient<MountainInsuranceOffersPage>();
            services.AddTransient<MountainInsuranceInfoPageViewModel>().AddTransient<MountainInsuranceInfoPage>();
            services.AddTransient<MountainInsuranceSummaryViewModel>().AddTransient<MountainInsuranceSummaryPage>();
            services.AddTransient<MountainInsuranceDocumentsViewModel>().AddTransient<MountainInsuranceDocumentsPage>();
            services.AddTransient<InsuredUsersPageViewModel>().AddTransient<InsuredUsersPage>();
            services.AddTransient<HealthInsuranceInsuredUsersViewModel>().AddTransient<HealthInsuranceInsuredUsersPage>();
            services.AddTransient<HealthInsurancePacketsPageViewModel>().AddTransient<HealthInsurancePacketsPage>();
            services.AddTransient<HealthInsurancePeriodPageViewModel>().AddTransient<HealthInsurancePeriodPage>();
            services.AddTransient<HealthInsuranceOffersPageViewModel>().AddTransient<HealthInsuranceOffersPage>();
            services.AddTransient<HealthDeclarationPageViewModel>().AddTransient<HealthDeclarationPage>();
            services.AddTransient<InsuranceOwnerPageViewModel>().AddTransient<InsuranceOwnerPage>();
            services.AddTransient<HealthInsuranceDocumentsPageViewModel>().AddTransient<HealthInsuranceDocumentsPage>();
            services.AddTransient<HealthInsuranceSummaryPageViewModel>().AddTransient<HealthInsuranceSummaryPage>();
            services.AddTransient<MyThingsCharacteristicsPageViewModel>().AddTransient<MyThingsCharacteristicsPage>();
            services.AddTransient<MyThingsInsuranceCategoryPageViewModel>().AddTransient<MyThingsInsuranceCategoryPage>();
            services.AddTransient<MyThingsInsuranceCalendarPageViewModel>().AddTransient<MyThingsInsuranceCalendarPage>();
            services.AddTransient<MyThingsInsuranceOffersViewModel>().AddTransient<MyThingsInsuranceOffersPage>();
            services.AddTransient<MyThingsInsuranceInfoPageViewModel>().AddTransient<MyThingsInsuranceInfoPage>();
            services.AddTransient<MyThingsInsuranceDocumentsPageViewModel>().AddTransient<MyThingsInsuranceDocumentsPage>();
            services.AddTransient<TravelInsuredUsersPageViewModel>().AddTransient<TravelInsuredUsersPage>();
            services.AddTransient<NewInsurancePageViewModel>().AddTransient<NewInsurancePage>();
            services.AddTransient<SuccessfulRegistrationPageViewModel>().AddTransient<SuccessfulRegistrationPage>();
            services.AddTransient<CivilInsuranceVehicleAdditionalInfoViewModel>().AddTransient<CivilInsuranceVehicleAdditionalInfoPage>();
            services.AddTransient<CivilInsuranceLongOffersViewModel>().AddTransient<CivilInsuranceLongOffersPage>();
            services.AddTransient<CascoInsuranceVehicleViewModel>().AddTransient<CascoInsuranceVehiclePage>();
            services.AddTransient<CascoInsuranceOwnerViewModel>().AddTransient<CascoInsuranceOwnerPage>();
            services.AddTransient<CascoInsuranceThankYouViewModel>().AddTransient<CascoInsuranceThankYouPage>();
            services.AddTransient<ScanOrFillVehicleDataPage, ScanOrFillVehicleDataPageViewModel>();
            services.AddTransient<CameraPage>();

            //Popups
            services.AddTransientPopup<EmailInputPopUp, EmailInputPopUpViewModel>();
            services.AddTransientPopup<SpeedyOfficesPopUp, SpeedyOfficesPopUpViewModel>();
            services.AddTransientPopup<MountainXtreemInfoPopUp, MountainXtreemInfoPopUpViewModel>();
            services.AddTransientPopup<HealthInsuranceContactUsPopUp, HealthInsuranceContactUsPopUpViewModel>();
        }
    }
}
